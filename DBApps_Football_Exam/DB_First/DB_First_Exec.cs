namespace DB_First
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using System.Globalization;
    using System.Threading;
    using System.Diagnostics;
    using System.Xml.XPath;
    using System.Data.Entity;

    public class DB_First_Exec
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
            var contex = new FootballEntities();

            Console.WriteLine("################################# Problem 1 #################################");
            var teamNames = contex.Teams.Select(t => t.TeamName).ToList();
            foreach (var name in teamNames)
            {
                Console.WriteLine(name);
            }

            Console.WriteLine();
            Console.WriteLine("################################# Problem 2 #################################");

            var leagueAndTeams = contex.Leagues.Select(l => new
            {
                leagueName = l.LeagueName,
                teams = l.Teams.Select(t => t.TeamName).OrderBy(t => t)
            })
            .OrderBy(l => l.leagueName)
            .ToList();

            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(leagueAndTeams);
            System.IO.File.WriteAllText("leagues-and-teams.json", json);
            Console.WriteLine("League and teams are exported");
            Console.WriteLine();

            Console.WriteLine("################################# Problem 3 #################################");

            var internationalMatches = contex.InternationalMatches.Select(im => new
            {
                homeCountryCode = im.HomeCountryCode,
                homeCountryName = im.HomeCountry.CountryName,
                homeCountryScore = im.HomeGoals,
                awayCountryCode = im.AwayCountryCode,
                awayCountryName = im.AwayCountry.CountryName,
                awayCountryScore = im.AwayGoals,
                date = im.MatchDate,
                league = im.League.LeagueName
            }).OrderBy(m => m.date)
                .ThenBy(m => m.homeCountryName)
                .ThenBy(m => m.awayCountryName)
                .ToList();

            var root = new XElement("matches");
            foreach (var im in internationalMatches)
            {
                var match = new XElement("match");
                if (im.date != null)
                {
                    if (im.date.Value.TimeOfDay == TimeSpan.Zero)
                    {
                        match.Add(new XAttribute("date", im.date.Value.ToString("dd-MMM-yyyy")));
                    }
                    else
                    {
                        match.Add(new XAttribute("date-time", im.date.Value.ToString("dd-MMM-yyyy hh:mm")));
                    }
                }
                match.Add(new XElement("home-country", im.homeCountryName, new XAttribute("code", im.homeCountryCode)));
                match.Add(new XElement("away-country", im.awayCountryName, new XAttribute("code", im.awayCountryCode)));
                if (im.homeCountryScore != null && im.awayCountryScore != null)
                {
                    match.Add(new XElement("score", im.homeCountryScore+"-"+im.awayCountryScore));
                }
                if (im.league != null)
                {
                    match.Add(new XElement("league", im.league));
                }
                root.Add(match);
            }
            var xmlFileToSave = new XDocument();
            xmlFileToSave.Add(root);
            xmlFileToSave.Save("international-matches.xml");

            Console.WriteLine("International matches are exported");
            Console.WriteLine();

            Console.WriteLine("################################# Problem 4 #################################");
            var xmlDoc = XDocument.Load("leagues-and-teams.xml");
            var leagues = xmlDoc.XPathSelectElements("/leagues-and-teams/league");
            int procLeague = 1;
            foreach (var league in leagues)
            {
                Console.WriteLine("Processing league #{0}",procLeague++);
                var newLeague = ProccessLeague(league, contex);
                var teams = league.XPathSelectElements("teams/team");
                ProcessTeams(contex, teams, newLeague);
                Console.WriteLine();
            }

            Console.WriteLine("Added leagues and teams");
            Console.WriteLine();

            Console.WriteLine("################################# Problem 5 #################################");
            var genMatches = XDocument.Load("generate-matches.xml");
            var genSpecs = genMatches.XPathSelectElements("/generate-random-matches/generate");
            int process = 1;
            foreach (var spec in genSpecs)
            {
                Console.WriteLine("Processing league #{0}", process++);
                var match = new GenMatch();
                GetSpecifications(contex, match, spec);
                GenerateMatches(contex, match);
                Console.WriteLine();
            }
        }

        private static void GenerateMatches(FootballEntities context, GenMatch match)
        {   
            var rnd = new Random();
            for (int i = 0; i < match.Count; i++)
            {
                var TeamMatch = new TeamMatch();
                Team homeTeam;
                Team awayTeam;
                if (match.League != null)
                {
                    TeamMatch.League = match.League;
                    var teams = match.League.Teams;
                    homeTeam = teams.ElementAt(rnd.Next(teams.Count));
                    awayTeam = teams.ElementAt(rnd.Next(teams.Count));
                    while (awayTeam.TeamName == homeTeam.TeamName)
                    {
                        awayTeam = teams.ElementAt(rnd.Next(teams.Count));
                    }
                }
                else
                {
                    var teams = context.Teams.ToList();
                    homeTeam = teams.ElementAt(rnd.Next(teams.Count));
                    awayTeam = teams.ElementAt(rnd.Next(teams.Count));
                    while (awayTeam.TeamName == homeTeam.TeamName)
                    {
                        awayTeam = teams.ElementAt(rnd.Next(teams.Count));
                    }
                }
                TeamMatch.HomeTeam = homeTeam;
                TeamMatch.AwayTeam = awayTeam;
                //generate random date
                int range = ((TimeSpan) (match.EndDate - match.StartDate)).Days;
                TeamMatch.MatchDate = match.StartDate.AddDays(rnd.Next(range));
                //generate goals
                int homeGoals = rnd.Next(match.Goals);
                int awayGoals = match.Goals - homeGoals;
                TeamMatch.HomeGoals = homeGoals;
                TeamMatch.AwayGoals = awayGoals;
                context.TeamMatches.Add(TeamMatch);
                context.SaveChanges();
                Console.WriteLine("{0}: {1} - {2}: {3} ({4})",
                    TeamMatch.MatchDate.Value.ToString("dd-MMM-yyyy"), 
                    TeamMatch.HomeTeam.TeamName, 
                    TeamMatch.AwayTeam.TeamName, 
                    TeamMatch.HomeGoals + "-" + TeamMatch.AwayGoals, 
                    TeamMatch.League != null ? TeamMatch.League.LeagueName : "no league");
            }
        }

        private static void GetSpecifications(FootballEntities context, GenMatch match, XElement el)
        {
            if (el.Attribute("generate-count") != null)
            {
                match.Count = int.Parse(el.Attribute("generate-count").Value);
            }
            if (el.Attribute("max-goals") != null)
            {
                match.Goals = int.Parse(el.Attribute("max-goals").Value);
            }
            if (el.Element("league") != null)
            {
                string leagueName = el.Element("league").Value;
                var league = context.Leagues.FirstOrDefault(l => l.LeagueName == leagueName);
                match.League = league;
            }
            if (el.Element("start-date") != null)
            {
                match.StartDate =  Convert.ToDateTime(el.Element("start-date").Value);
            }
            if (el.Element("end-date") != null)
            {
                match.EndDate = Convert.ToDateTime(el.Element("end-date").Value);
            }
        }

        private class GenMatch
        {
            public GenMatch()
            {
                this.League = null;
                this.Count = 10;
                this.Goals = 5;
                this.StartDate = new DateTime(2000, 1, 1);
                this.EndDate = new DateTime(2015, 12, 31);
            }

            public League League { get; set; }
            public int Count { get; set; }
            public int Goals { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        private static void ProcessTeams(FootballEntities context, IEnumerable<XElement> teams, League newLeague)
        {
            foreach (var team in teams)
            {
                var teamName = team.Attribute("name").Value;
                var country = team.Attribute("country");
                string countryName = null;
                if (country != null)
                {
                    countryName = country.Value;
                }

                var findTeam =
                    context.Teams.Include(t => t.Leagues)
                        .FirstOrDefault(t => t.TeamName == teamName 
                            && t.Country.CountryName == countryName);

                if (findTeam != null)
                {
                    Console.WriteLine("Existing team: {0} ({1})", findTeam.TeamName, countryName ?? "no country");
                }
                else
                {
                    findTeam = new Team()
                    {
                        TeamName = teamName,
                        Country = context.Countries.FirstOrDefault(c => c.CountryName == countryName)
                    };
                    context.Teams.Add(findTeam);
                    context.SaveChanges();
                    Console.WriteLine("Created team: {0} ({1})", teamName, countryName ?? "no country");
                }

                AddTeamToLeague(context, findTeam, newLeague);
            }
        }

        private static void AddTeamToLeague(FootballEntities context, Team findTeam, League newLeague)
        {
            if (newLeague != null)
            {
                if (findTeam.Leagues.Contains(newLeague))
                {
                    Console.WriteLine("Existing team in league: {0} belongs to {1}",
                        findTeam.TeamName, newLeague.LeagueName);
                }
                else
                {
                    findTeam.Leagues.Add(newLeague);
                    context.SaveChanges();
                    Console.WriteLine("Added team in league: {0} to league {1}", findTeam.TeamName, newLeague.LeagueName);
                }
            }
        }

        private static League ProccessLeague(XElement league, FootballEntities context)
        {
            League newLeague = null;
            if (league.Element("league-name") != null)
            {
                var leagueName = league.Element("league-name").Value;
                newLeague = context.Leagues.FirstOrDefault(l => l.LeagueName == leagueName);
                if (newLeague != null)
                {
                    Console.WriteLine("Existing league: {0}", leagueName);
                }
                else
                {
                    newLeague = new League() { LeagueName = leagueName };
                    context.Leagues.Add(newLeague);
                    context.SaveChanges();
                    Console.WriteLine("Created league: {0}", leagueName);
                }
            }
            return newLeague;
        }
    }
}
