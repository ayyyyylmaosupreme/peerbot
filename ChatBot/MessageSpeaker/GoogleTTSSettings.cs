﻿using Google.Cloud.TextToSpeech.V1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChatBot.MessageSpeaker
{
    public static class GoogleTTSSettings
    {
        private static int requestCounter = 0;
        private static int maximumRequests = 1250;

        public enum eDialects
        {
            australian,
            italian,
            german,
            british,
            american,
            french,
            japanese,
            canadianFrench,
            korean,
            chinese,
            russian,
            danish,
            none
        }

        public enum voicePresets
        {
            frenchWoman,
            frenchMan,
            bog
        }

        public static TextToSpeechClient GetTTSClient()
        {
            TextToSpeechClient ttsClient = TextToSpeechClient.Create();
            return ttsClient;
        }

        /// <summary>
        /// Returns a memory stream with a wav header of a user's message in their custom voice
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static MemoryStream GetVoiceAudio(string username, string message, TextToSpeechClient client, int requestCount)
        {
            requestCounter += requestCount;

            if (requestCounter > maximumRequests)
                return null;

            TTSSettings ttsSettings = UserTTSSettingsManager.GetSettingsFromStorage(username).ttsSettings;
            SynthesizeSpeechRequest request = GoogleTTSSettings.GetSpeechRequest(message, ttsSettings);
            SynthesizeSpeechResponse response = client.SynthesizeSpeech(request);

            MemoryStream outputMemoryStream = new MemoryStream();
            response.AudioContent.WriteTo(outputMemoryStream);

            return outputMemoryStream;
        }

        public static string GetLanguageCodeFromDialect(string dialect)
        {
            switch (GetDialectFromString(dialect))
            {
                case eDialects.australian:
                    return "en-AU";
                case eDialects.danish:
                    return "da-DK";
                case eDialects.russian:
                    return "ru-RU";
                case eDialects.chinese:
                    return "cmn-TW";
                case eDialects.korean:
                    return "ko-KR";
                case eDialects.british:
                    return "en-GB";
                case eDialects.italian:
                    return "it-IT";
                case eDialects.german:
                    return "de-DE";
                case eDialects.american:
                    return "en-US";
                case eDialects.french:
                    return "fr-FR";
                case eDialects.japanese:
                    return "ja-JP";
                case eDialects.canadianFrench:
                    return "fr-CA";
                default:
                    return "";
            }
        }

        private static VoiceSelectionParams GetVoiceParams(SsmlVoiceGender gender, string languageCode, string voiceName)
        {
            VoiceSelectionParams voiceParams = new VoiceSelectionParams();
            voiceParams.LanguageCode = languageCode;
            voiceParams.SsmlGender = gender;
            voiceParams.Name = voiceName;

            return voiceParams;
        }

        private static SsmlVoiceGender ConvertGender(TTSSettings.eGender gender)
        {
            if (gender == TTSSettings.eGender.male)
                return SsmlVoiceGender.Male;
            else if (gender == TTSSettings.eGender.female)
                return SsmlVoiceGender.Female;

            return SsmlVoiceGender.Female;
        }

        public static string GetVoiceNameFromLanguageCode(string languageCode, SsmlVoiceGender gender)
        {
            // Hand picked voice sounds by region - if the enum isnt covered in this list just use the default voice name
            if (gender == SsmlVoiceGender.Male)
            {
                switch (GetDialectFromLanguageCode(languageCode))
                {
                    case eDialects.australian:
                        return "en-AU-Wavenet-D";
                    case eDialects.danish:
                        return "da-DK-Standard-C";
                    case eDialects.russian:
                        return "ru-RU-Wavenet-D";
                    case eDialects.chinese:
                        return "cmn-TW-Wavenet-C";
                    case eDialects.korean:
                        return "ko-KR-Wavenet-C";
                    case eDialects.british:
                        return "en-GB-Wavenet-D";
                    case eDialects.italian:
                        return "it-IT-Wavenet-D";
                    case eDialects.german:
                        return "de-DE-Standard-D";
                    case eDialects.american:
                        return "en-US-Standard-D";
                    case eDialects.french:
                        return "fr-FR-Wavenet-B";
                    case eDialects.japanese:
                        return "ja-JP-Wavenet-D";
                    case eDialects.canadianFrench:
                        return "fr-CA-Standard-D";
                    default:
                        return "";
                }
            }
            else if (gender  == SsmlVoiceGender.Female)
            {
                switch (GetDialectFromLanguageCode(languageCode))
                {
                    case eDialects.australian:
                        return "en-AU-Wavenet-C";
                    case eDialects.danish:
                        return "da-DK-Wavenet-A";
                    case eDialects.russian:
                        return "ru-RU-Wavenet-C";
                    case eDialects.chinese:
                        return "cmn-TW-Wavenet-A";
                    case eDialects.korean:
                        return "ko-KR-Wavenet-A";
                    case eDialects.british:
                        return "en-GB-Wavenet-F";
                    case eDialects.italian:
                        return "it-IT-Wavenet-A";
                    case eDialects.german:
                        return "de-DE-Wavenet-A";
                    case eDialects.american:
                        return "en-US-Wavenet-F";
                    case eDialects.french:
                        return "fr-FR-Wavenet-A";
                    case eDialects.japanese:
                        return "ja-JP-Wavenet-B";
                    default:
                        return "";
                }
            }
            else return "";
        }

        public static eDialects GetDialectFromLanguageCode(string languageCode)
        {
            switch (languageCode)
            {
                case "en-GB":
                    return eDialects.british;
                case "da-DK":
                    return eDialects.danish;
                case "ru-RU":
                    return eDialects.russian;
                case "ko-KR":
                    return eDialects.korean;
                case "cmn-TW":
                    return eDialects.chinese;
                case "it-IT":
                    return eDialects.italian;
                case "de-DE":
                    return eDialects.german;
                case "en-AU":
                    return eDialects.australian;
                case "en-US":
                    return eDialects.american;
                case "fr-FR":
                    return eDialects.french;
                case "ja-JP":
                    return eDialects.japanese;
                case "fr-CA":
                    return eDialects.canadianFrench;
                default:
                    return eDialects.french;
            }
        }

        public static eDialects GetDialectFromString(string dialect)
        {
            switch (dialect.ToLower())
            {
                case "australian":
                case "australia":
                case "aus":
                    return eDialects.australian;
                case "chinese":
                    return eDialects.chinese;
                case "danish":
                    return eDialects.danish;
                case "korean":
                    return eDialects.korean;
                case "russian":
                    return eDialects.russian;
                case "italian":
                case "italy":
                    return eDialects.italian;
                case "german":
                case "germany":
                    return eDialects.german;
                case "british":
                case "uk":
                    return eDialects.british;
                case "american":
                case "america":
                    return eDialects.american;
                case "french":
                case "france":
                    return eDialects.french;
                case "japanese":
                case "japan":
                    return eDialects.japanese;
                case "canadian french":
                    return eDialects.canadianFrench;
                default:
                    return eDialects.none;
            }
        }


        private static AudioConfig GetAudioConfig(double speakingRate, double pitch)
        {
            AudioConfig audioConfig = new AudioConfig();
            audioConfig.AudioEncoding = AudioEncoding.Linear16;
            audioConfig.SpeakingRate = speakingRate;
            audioConfig.Pitch = pitch;
            return audioConfig;
        }

        /// <summary>
        /// Returns speech request with audio as linear16 with wav header
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ttsSettings"></param>
        /// <returns></returns>
        private static SynthesizeSpeechRequest GetSpeechRequest(string message, TTSSettings ttsSettings)
        {
            // Raise the speaking rate of long messages so they play faster
            if (message.Length > 40 && ttsSettings.speakingRate < 2)
            {
                ttsSettings.speakingRate += 0.33;
                ttsSettings.pitch += 1;
            }
            if (message.Length > 55 && ttsSettings.speakingRate < 2)
            {
                ttsSettings.speakingRate += 0.33;
                ttsSettings.pitch += 1.5;
            }
            if (message.Length > 65 && ttsSettings.speakingRate < 2)
            {
                ttsSettings.speakingRate += 0.33;
                ttsSettings.pitch += 2;
            }

            // Build the audio request
            AudioConfig config = GetAudioConfig(ttsSettings.speakingRate, ttsSettings.pitch);

            var voiceParams = GetVoiceParams(ConvertGender(ttsSettings.gender),
                ttsSettings.languageCode, ttsSettings.voiceName);

            SynthesizeSpeechRequest request = new SynthesizeSpeechRequest();
            request.AudioConfig = config;
            request.Voice = voiceParams;

            // Add the text message to the audio request
            SynthesisInput input = new SynthesisInput();
            input.Text = message;
            request.Input = input;

            return request;
        }

        /// <summary>
        /// Gives a user a handmade preconfigurated voice
        /// </summary>
        /// <param name="username"></param>
        /// <param name="voicePreset"></param>
        public static void SetPresetVoice(string username, voicePresets voicePreset)
        {
            UserTTSSettings userTTSSettings = new UserTTSSettings();
            userTTSSettings.twitchUsername = username;
            userTTSSettings.isSpeechEnabled = true;

            switch (voicePreset)
            {
                case voicePresets.frenchWoman:
                    userTTSSettings.ttsSettings.speakingRate = 1.16;
                    userTTSSettings.ttsSettings.pitch = -1.3;
                    userTTSSettings.ttsSettings.gender = TTSSettings.eGender.female;
                    userTTSSettings.ttsSettings.languageCode = "fr-FR";
                    break;
                case voicePresets.bog:
                    userTTSSettings.ttsSettings.speakingRate = 0.58;
                    userTTSSettings.ttsSettings.pitch = -3;
                    userTTSSettings.ttsSettings.gender = TTSSettings.eGender.male;
                    userTTSSettings.ttsSettings.languageCode = "fr-CA";
                    break;
                case voicePresets.frenchMan:
                    userTTSSettings.ttsSettings.speakingRate = 1.17;
                    userTTSSettings.ttsSettings.pitch = -2;
                    userTTSSettings.ttsSettings.gender = TTSSettings.eGender.male;
                    userTTSSettings.ttsSettings.languageCode = "fr-FR";
                    break;
            }

            UserTTSSettingsManager.SaveSettingsToStorage(userTTSSettings);
        }
    }
}
