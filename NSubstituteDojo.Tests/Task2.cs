﻿using System;
using NSubstitute;
using NUnit.Framework;

namespace NSubstituteDojo.Tests
{
    public class Task2
    {
        private Badger _badger;

        [SetUp]
        public void Setup()
        {
            _badger = new Badger(Guid.NewGuid(), "Boris");
        }

        [Test] // STUBS
        public async void UpdatingABadgerWithTheSameNameUsingStubs()
        {
            var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
            findBadgerByIdQuery
				.FindById(_badger.Id)
				.Returns(_badger);

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, Substitute.For<IUpdateBadgerNameCommand>());

            var result = await service.ChangeName(_badger.Id, "Boris");

            Assert.That(result.Status, Is.EqualTo(ChangeBadgerNameService.UpdateStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Boris"));
        }

        [Test] // MOCKS
        public async void UpdatingABadgerWithTheSameNameUsingMocks()
        {
            var findBadgerByIdQuery = Substitute.For<IFindBadgerByIdQuery>();
            findBadgerByIdQuery
				.FindById(Arg.Any<Guid>())
				.Returns(_badger);

            IUpdateBadgerNameCommand updateBadgerNameCommand = null;

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

            var result = await service.ChangeName(_badger.Id, "Boris");

            await findBadgerByIdQuery.Received(1).FindById(_badger.Id);
            await updateBadgerNameCommand.Received(0).Update(Arg.Any<Guid>(), "Boris");

            Assert.That(result.Status, Is.EqualTo(ChangeBadgerNameService.UpdateStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Boris"));
        }

        [Test] // STUBS
        public async void UpdatingABadgerWithAValidNameUsingStubs()
        {
            IFindBadgerByIdQuery findBadgerByIdQuery = null;
            IUpdateBadgerNameCommand updateBadgerNameCommand = null;

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

            var result = await service.ChangeName(_badger.Id, "Brock");

            Assert.That(result.Status, Is.EqualTo(ChangeBadgerNameService.UpdateStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));

        }

        [Test] // MOCKS
        public async void UpdatingABadgerWithAValidNameUsingMocks()
        {
            IFindBadgerByIdQuery findBadgerByIdQuery = null;
            IUpdateBadgerNameCommand updateBadgerNameCommand = null;

            var service = new ChangeBadgerNameService(findBadgerByIdQuery, updateBadgerNameCommand);

            var result = await service.ChangeName(_badger.Id, "Brock");

            await findBadgerByIdQuery.Received(1).FindById(_badger.Id);
            await updateBadgerNameCommand.Received(1).Update(_badger.Id, "Brock");

            Assert.That(result.Status, Is.EqualTo(ChangeBadgerNameService.UpdateStatus.Ok));
            Assert.That(result.UpdatedBadger.Name, Is.EqualTo("Brock"));
        }
    }
}