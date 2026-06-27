using System;
using System.Collections.Generic;
using System.Text;
using Supabase;

namespace NoorSound.Services
{
    public class StartupService : IStartupService
    {
        private readonly Client _client;

        public StartupService(Client client)
        {
            _client = client;
        }

        public async Task InitializeAsync()
        {
            await _client.InitializeAsync();
        }
    }
}
