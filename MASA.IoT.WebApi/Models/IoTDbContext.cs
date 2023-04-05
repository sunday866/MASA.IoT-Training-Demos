using Microsoft.EntityFrameworkCore;

namespace MASA.IoT.WebApi.Models
{
    public class IoTDbContext : MasaDbContext<IoTDbContext>
    {
        public IoTDbContext(MasaDbContextOptions<IoTDbContext> options) : base(options)
        {
        }

        public virtual DbSet<IoTDeviceInfo> IoTDeviceInfo { get; set; }
        public virtual DbSet<IoTProductInfo> IoTProductInfo { get; set; }
    }
}
