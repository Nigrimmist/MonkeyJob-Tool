using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;

using HelloBotCommunication;


namespace SmartAssHandlerLib
{
    public class MoviesHandlerBase : ModuleBase
    {
        private IBot _bot;

        public override void Init(IBot bot)
        {
            _bot = bot;
        }

        public override double ModuleVersion
        {
            get { return 1.0; }
        }

        public override ReadOnlyCollection<CallCommandInfo> CallCommandList
        {
            get
            {
                return new ReadOnlyCollection<CallCommandInfo>(new List<CallCommandInfo>()
                {
                    new CallCommandInfo("кинопоиск",new List<string>(){"кино"})
                });
            }
        }

        public override string CommandDescription
        {
            get { return string.Empty; }
        }

        public override void HandleMessage(string command, string args, Guid commandToken)
        {
            var request = BuildRequestUrl(args);
            var rawData = GetRawData(request);
            var description = ParseMovieData(rawData);

            _bot.ShowMessage(commandToken,description.ToString());
        }

        private MovieDescription ParseMovieData(string rawData)
        {
            return new MovieDescription();
        }

        private string GetRawData(string location)
        {
            var client = new WebClient();

            var rawResult = client.DownloadData(location);
            var chars = new char[rawResult.Length];

            Encoding.UTF8.GetDecoder().GetChars(rawResult, 0, rawResult.Length, chars, 0);

            return new string(chars);
        }

        private string BuildRequestUrl(string args)
        {
            return string.Empty;
        }

        private class MovieDescription
        {
            public string Title { get; set; }
            public string Year { get; set; }
            public string Genre { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return string.Format("Название: {0}{1}Год выпуска: {2}{3}Жанр: {4}{5}Описание: \"{6}\"",
                    Title,
                    Environment.NewLine,
                    Year,
                    Environment.NewLine,
                    Genre,
                    Environment.NewLine,
                    Description);
            }
        }
    }
}
