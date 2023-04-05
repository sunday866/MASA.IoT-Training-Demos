using Google.Api;
using Masa.BuildingBlocks.Data.UoW;
using Masa.BuildingBlocks.Ddd.Domain.Repositories;
using Masa.Contrib.Ddd.Domain.Repository.EFCore;
using MASA.IoT.WebApi.Domain.IRepositories;
using MASA.IoT.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MASA.IoT.WebApi.Infrastructure.Repositories
{
    public class IoTDeviceInfoRepostiory: Repository<IoTDbContext, IoTDeviceInfo>, IIoTDeviceInfoRepostiory
    {

        public IoTDeviceInfoRepostiory(IoTDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }

    }
}
