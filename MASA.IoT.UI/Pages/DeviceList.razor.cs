using BlazorComponent;
using MASA.IoT.Common.Helper;
using MASA.IoT.Core.Contract.Device;
using MASA.IoT.UI.Caller;
using Microsoft.AspNetCore.Components;

namespace MASA.IoT.UI.Pages
{
    public partial class DeviceList : ComponentBase
    {
        StringNumber _tabIndex;
        private object _optionECharts = new();

        private int _totalCount = 0;
        private MqttHelper mqttHelper { get; set; }
        private List<DeviceListViewModel> deviceList { get; set; } = new();
        private bool ShowDrawer { get; set; }
        [Inject]
        private DeviceCaller _deviceCaller { get; set; }

        private readonly DeviceListOption _options = new()
        {
            PageIndex = 1,
            PageSize = 10,
        };

        private List<DataTableHeader<DeviceListViewModel>> _headers = new()
        {
           new DataTableHeader<DeviceListViewModel>
           {
                Text= "设备名称",
                Align= DataTableHeaderAlign.Start,
                Sortable= false,
                Value= nameof(DeviceListViewModel.DeviceName)
           },
          new DataTableHeader<DeviceListViewModel>
           {
              Text= "在线状态",
              Align= DataTableHeaderAlign.Start,
              Sortable= false,
              Value= nameof(DeviceListViewModel.OnLineStates)
           },
           new DataTableHeader<DeviceListViewModel>
           {
               Text= "Actions", 
               Value= "actions",
               Sortable=false,
               Width="100px",
               Align=DataTableHeaderAlign.Center,
           }
         };

        private async Task DrawerChangedAsync()
        {
            ShowDrawer = !ShowDrawer;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var paginatedList = await _deviceCaller.DeviceListAsync(new DeviceListOption { PageIndex = 1, PageSize = 10, ProductId = new Guid("C85EF7E5-2E43-4BD2-A939-07FE5EA3F459") });
                deviceList = paginatedList.Result.ToList();
                _totalCount = (int)paginatedList.Total;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task EditItem(DeviceListViewModel item)
        {
            var eChartsData = await _deviceCaller.GetDeviceDataPointList(new GetDeviceDataPointListOption { ProductId = Guid.Parse("c85ef7e5-2e43-4bd2-a939-07fe5ea3f459"), DeviceName = item.DeviceName, StartDateTime = DateTime.Today, StopDateTime = DateTime.Today.AddDays(1) });
            if (eChartsData != null)
            {
                _optionECharts = GetEChartsData(eChartsData);
            }
            ShowDrawer = true;
        }

        private async Task TabsValueChanged(StringNumber value)
        {
            _tabIndex = value;
        }

        private dynamic GetEChartsData(EChartsData data)
        {
            return new
            {
                tooltip = new
                {
                    trigger = "axis"
                },
                legend = new
                {
                    Data = new[] { "Pm2.5", "Humidity", "Temperature" }
                },
                XAxis = new
                {
                    Type = "category",
                    Data = data.FieldDataList.First().DateTimes.Select(o => o.ToLocalTime().ToString("t"))
                },
                YAxis = new
                {
                    Min = 10,
                    Max = 100,
                    Type = "value",
                },
                Series = new[]
                {
                new
                {
                    Name ="Pm2.5",
                    Type = "line",
                    Smooth = true,
                    Data = data.FieldDataList.First(o => o.FieldName=="PM_25").Values
                },
                new
                {
                    Name ="Humidity",
                    Type = "line",
                    Smooth = true,
                    Data = data.FieldDataList.First(o => o.FieldName=="Humidity").Values
                },
                new
                {
                    Name ="Temperature",
                    Type = "line",
                    Smooth = true,
                    Data = data.FieldDataList.First(o => o.FieldName=="Temperature").Values.Select(o => o-5).ToList()
                }
            }
            };
        }
    }
}
