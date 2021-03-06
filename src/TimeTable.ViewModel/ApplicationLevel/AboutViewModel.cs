﻿using System;
using System.Windows.Input;
using JetBrains.Annotations;
using Microsoft.Phone.Tasks;
using TimeTable.Mvvm;
using TimeTable.ViewModel.Commands;
using TimeTable.ViewModel.Services;

namespace TimeTable.ViewModel.ApplicationLevel
{
    public class AboutViewModel : PageViewModel
    {
        private readonly FlurryPublisher _flurryPublisher;
        private const string URL = "http://raspisaniye-vuzov.ru/mobile/";
        private const string TWITTER = "https://twitter.com/raspisanievuzov";

        public AboutViewModel([NotNull] FlurryPublisher flurryPublisher)
        {
            if (flurryPublisher == null) throw new ArgumentNullException("flurryPublisher");
            _flurryPublisher = flurryPublisher;
            _flurryPublisher.PublishPageLoadedAbout();
            InitCommands();
        }

        private void InitCommands()
        {
            ShowMobileSiteCommand = new SimpleCommand(ShowWebSite);
            ShowTwitterCommand = new SimpleCommand(ShowTwitter);
        }

        private void ShowWebSite()
        {
            _flurryPublisher.PublishShowMobile();
            var webBrowserTask = new WebBrowserTask {Uri = new Uri(URL)};
            webBrowserTask.Show();
        }

        private void ShowTwitter()
        {
            var webBrowserTask = new WebBrowserTask { Uri = new Uri(TWITTER) };
            webBrowserTask.Show();
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public string Version
        {
            get { return Configuration.Version; }
        }

        [UsedImplicitly(ImplicitUseKindFlags.Access)]
        public ICommand ShowMobileSiteCommand { get; private set; }
        public ICommand ShowTwitterCommand { get; private set; }
    }
}