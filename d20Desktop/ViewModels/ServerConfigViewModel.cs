﻿using Fiction.GameScreen.Server;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace Fiction.GameScreen.ViewModels
{
    /// <summary>
    /// View model for configuring a server connection
    /// </summary>
    public sealed class ServerConfigViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Constructs a new <see cref="ServerConfigViewModel"/>
        /// </summary>
        /// <param name="factory">View model factory to update</param>
        /// <param name="campaignName">Campaign name to use</param>
        public ServerConfigViewModel(ViewModelFactory factory, string campaignName)
        {
            _modelFactory = factory;
            _campaign = factory.Campaign;
            _serverUri = factory.Campaign.ServerUri;
            _originalUri = _serverUri;
            _campaignName = campaignName;
        }
        private CampaignSettings _campaign;
        private ViewModelFactory _modelFactory;

        private string _campaignName;
        /// <summary>
        /// Gets or sets the name to use for the campaign
        /// </summary>
        public string CampaignName
        {
            get { return _campaignName; }
            set
            {
                if (!string.Equals(_campaignName, value, StringComparison.Ordinal))
                {
                    _campaignName = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CampaignName)));
                }
            }
        }

        private string? _originalUri;
        private string? _serverUri;
        /// <summary>
        /// Gets or sets the server URI to use for campaign and combat management
        /// </summary>
        public string? ServerUri
        {
            get { return _serverUri; }
            set
            {
                if (!string.Equals(_serverUri, value, StringComparison.Ordinal))
                {
                    _serverUri = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ServerUri)));
                }
            }
        }

        /// <summary>
        /// Saves the settings to the campaign and gets a campaign ID
        /// </summary>
        public async Task Save()
        {
            if (!string.IsNullOrWhiteSpace(_serverUri))
            {
                if (!string.Equals(_serverUri, _originalUri, StringComparison.OrdinalIgnoreCase))
                {
                    HttpClient client = new HttpClient()
                    {
                        BaseAddress = new Uri(_serverUri),
                    };

                    CampaignManagement server = new CampaignManagement(client);
                    string id = await server.CreateCampaign(_campaignName);
                    _campaign.CampaignID = id;
                    _campaign.ServerUri = _serverUri;
                    _modelFactory.SetServer(server);
                }
            }
            else
                _campaign.ServerUri = null;
        }

        /// <summary>
        /// Event that is triggered when a property is changed
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}