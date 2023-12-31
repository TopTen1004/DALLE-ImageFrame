﻿using dalleframecon.Configuration;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace dalleframecon.Handlers
{
    /// <summary>
    /// A speaker using Azure Cognitive Services text-to-speech.
    /// </summary>
    internal class AzCognitiveServicesSpeaker : IDisposable
    {
        private readonly AzureCognitiveServicesOptions _options;
        private readonly ILogger<AzCognitiveServicesSpeaker> _logger;
        private readonly SpeechSynthesizer _speechSynthesizer;

        public AzCognitiveServicesSpeaker(
            IOptions<AzureCognitiveServicesOptions> options,
            ILogger<AzCognitiveServicesSpeaker> logger)
        {
            _logger = logger;
            _options = options.Value;
            _options.Validate();

            SpeechConfig speechConfig = SpeechConfig.FromSubscription(_options.Key, _options.Region);
            speechConfig.SpeechSynthesisVoiceName = _options.SpeechSynthesisVoiceName;
            _speechSynthesizer = new SpeechSynthesizer(speechConfig);

        }

        /// <summary>
        /// Speak a message.
        /// </summary>
        public async Task SpeakAsync(string message, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                await _speechSynthesizer.SpeakTextAsync(message);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // TODO Disabling while we track down why speech synthesis is stalling.
            //_speechSynthesizer.Dispose();
        }
    }
}