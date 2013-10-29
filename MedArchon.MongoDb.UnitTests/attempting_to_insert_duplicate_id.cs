using System;
using System.Collections.Generic;
using Given.Common;
using Given.NUnit;
using log4net;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace MedArchon.Data.MongoDb.UnitTests
{
    public class mongo_repository_does_not_find_item_to_remove : Scenario
    {
        static MongoRepository _mongoRepository;
        static Mock<ILog> _mockLog;
        static Guid _guid;
        static Mock<IMongoCollection<object>> _mockMongoCollection;

        given a_mongo_repository = () =>
        {
            _mockMongoCollection = new Mock<IMongoCollection<object>>();
            _mockMongoCollection.Setup(x => x.Remove(It.IsAny<QueryDocument>()))
                .Returns(new WriteConcernResult(new BsonDocument(new Dictionary<string, object> {{"n", 0}})));
            _mockLog = new Mock<ILog>();
            _mongoRepository = new MongoRepository(null, _mockLog.Object);
        };

        when deleting_missing_id = () =>
        {
            _guid = Guid.NewGuid();
            _mongoRepository.Remove(_guid, _mockMongoCollection.Object);
        };

        [then]
        public void verify_non_delete_was_logged()
        {
            _mockLog.Verify(x => x.InfoFormat(MongoRepository.CannotRemoveInfoMessage, typeof(object), _guid));
        }
    }
}
