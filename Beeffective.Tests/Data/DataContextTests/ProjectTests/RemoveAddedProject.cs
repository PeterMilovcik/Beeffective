﻿using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Beeffective.Tests.Data.DataContextTests.ProjectTests
{
    class RemoveAddedProject : TestFixture
    {
        public override void SetUp()
        {
            base.SetUp();
            CreateGoal();
            CreateProject(NewGoal);
            SUT.Projects.Add(NewProject);
            SUT.SaveChanges();
            SUT.Projects.Remove(NewProject);
            SUT.SaveChanges();
        }

        [Test]
        public void AddedProjectIsRemovedSuccessfully() =>
            SUT.Projects
                .SingleOrDefault(entity =>
                    entity.Title == NewProject.Title &&
                    entity.Description == NewProject.Description &&
                    entity.Goal == NewProject.Goal)
                .Should().BeNull();
    }
}