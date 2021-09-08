using Cubo.Core.Domain;
using Cubo.Core.Repositories;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Cubo.Test
{
    [TestFixture]
    public class BucketRepositoryTests
    {
        [Test]
        public async Task Add_async_should_create_new_bucket()
        {
            //Arrange
            var repository = new InMemoryBucketRepository();
            var name = "test";
            var bucket = new Bucket(Guid.NewGuid(), name);

            //Act
            await repository.AddAsync(bucket);

            //Assert
            var createdBucket = await repository.GetAsync(name);
            Assert.AreSame(bucket, createdBucket);

           bucket.Equals(createdBucket);
        }
    }
}


