namespace testankiety.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using testankiety.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<testankiety.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(testankiety.Models.ApplicationDbContext context)
        {
            var qst = new List<YesNoQuestion>
            {
               new YesNoQuestion{ID=1, Text="pytanie1",Type=QuestionType.YesNo, Answer=true },
               new YesNoQuestion{ID=2, Text="pytanie2",Type=QuestionType.YesNo, Answer=false }

    };
            qst.ForEach(s => context.YesNoQuestions.AddOrUpdate(p => p.ID, s));

            context.SaveChanges();

            var usr = new List<UserAcc>
            {
               // new UserAcc{UserAccID=1,LastName="jakienazwisko1",EnrollmentDate=DateTime.Parse("2001-10-10"),
               new UserAcc{UserAccID=2,LastName="jakienazwisko",EnrollmentDate=DateTime.Parse("2001-10-10")}

    };
            usr.ForEach(s => context.UserAccs.AddOrUpdate(p => p.UserAccID, s));

            context.SaveChanges();

            var sur = new List<Survay>
            {
                //new Survay{SurvayID=1,Title="Ankieta1",UserAccRefId=1},
                //new Survay{SurvayID=2,Title="Ankieta2",UserAccRefId=1},
                //new Survay{SurvayID=4,Title="asas",UserAccRefId=1},
                //new Survay{SurvayID=20,Title="Inny user ankieta2",UserAccRefId=2},
                new Survay{SurvayID=16,Title="ankieta z question lista",YesNoQuestions=qst ,UserAccRefId=2}

            };
            sur.ForEach(s => context.Survays.AddOrUpdate(p => p.Title, s));

            context.SaveChanges();

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
