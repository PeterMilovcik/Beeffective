using System;
using Beeffective.Data.Entities;

namespace Beeffective.Tests.Builders
{
    public class TaskEntityBuilder
    {
        public const int TestId = 1;
        public const string TestTitle = "Test Title";
        public const string TestGoal = "Test Goal";
        public const string TestTags = "TestTag1 TestTag2";
        private const int TestImportance = 5;
        private const int TestUrgency = 3;
        public readonly DateTime? TestDueTo = new DateTime(2020, 05, 24, 13, 38, 27);

        private int id;
        private string title;
        private string goal;
        private string tags;
        private DateTime? dueTo;
        private int importance;
        private int urgency;
        private bool isFinished;

        public TaskEntityBuilder()
        {
            id = TestId;
            title = TestTitle;
            goal = TestGoal;
            tags = TestTags;
            dueTo = TestDueTo;
            importance = TestImportance;
            urgency = TestUrgency;
            isFinished = false;
        }

        public TaskEntityBuilder WithId(int newId)
        {
            id = newId;
            return this;
        }

        public TaskEntityBuilder WithTitle(string newTitle)
        {
            title = newTitle;
            return this;
        }

        public TaskEntityBuilder WithGoal(string newGoal)
        {
            goal = newGoal;
            return this;
        }

        public TaskEntityBuilder WithTags(string newTags)
        {
            tags = newTags;
            return this;
        }

        public TaskEntityBuilder WithDueTo(DateTime? newDueTo)
        {
            dueTo = newDueTo;
            return this;
        }

        public TaskEntityBuilder WithImportance(int newImportance)
        {
            importance = newImportance;
            return this;
        }

        public TaskEntityBuilder AsFinished()
        {
            isFinished = true;
            return this;
        }

        public TaskEntityBuilder WithUrgency(int newUrgency)
        {
            urgency = newUrgency;
            return this;
        }

        public TaskEntity Create() =>
            new TaskEntity
            {
                Id = id,
                Title = title,
                Goal = goal,
                DueTo = dueTo,
                Importance = importance,
                Urgency = urgency,
                IsFinished = isFinished,
                Tags = tags
            };
    }
}