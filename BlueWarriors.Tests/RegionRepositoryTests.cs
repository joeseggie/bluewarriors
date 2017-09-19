using System.Data.SqlClient;
using System.Threading.Tasks;
using Bluewarriors.Mvc.Repository;
using BlueWarriors.Mvc.Models;
using BlueWarriors.Mvc.Repository;
using Moq;
using Xunit;

namespace BlueWarriors.Tests
{
    public class RegionRepositoryTests
    {
        [Fact]
        [Trait("Category","RegionRepository")]
        public async Task TestName()
        {
            //Given
            var mock = new Mock<IDatabaseConnection>();
            mock.Setup(db => db.GetConnection()).Returns(new SqlConnection($"Data Source=172.25.0.26;Initial Catalog=BW;User Id=pps;Password=pps1234;"));
            var target = new RegionRepository<Region>(mock.Object);
            
            //When
            await target.AddAsync(new Region{Name = "Test"});
            
            //Then
            mock.Verify(foo => foo.GetConnection());
        }
    }
}