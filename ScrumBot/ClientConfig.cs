﻿using Microsoft.Extensions.Configuration;

namespace ScrumBot
{
    public class ClientConfig
    {
        public ClientConfig(IConfiguration configuration)
        {
            _token = configuration.GetConnectionString("DiscordClientSecret");
        }

        private string _token { get; }
        public string Prefix => "?";
        public string Token => _token;
    }
}