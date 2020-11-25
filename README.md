# MicroServiceGateway
MicroService 分成MicroServiceGateway.Service.Host、MicroServiceGateway.Manager.Host和MicroServiceGateway.Client三部分构成，其中MicroServiceGateway.Service.Host、MicroServiceGateway.Manager.Host只需要和redis服务器配置正确后即可使用，MicroServiceGateway.Client则需要嵌入到具体的服务项目中，目前项目正处于完善阶段。

## MicroServiceGateway.Client使用步骤

### 1.新建asp.net core 3或以上项目

### 2.nuget引用包，在nuget包管理器中搜索MicroServiceGateway.Client，或者程序包管理器控制台中输入：Install-Package MicroServiceGateway.Client -Version 1.1.2.2

### 3.在Startup.cs中添加app.UseMSGMiddleware(); 按如下编码

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroServiceGateway.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //取消https，暂不支持
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //use MicroServiceGateway middleware
            app.UseMSGMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

```

### 4.添加或修改MicroServiceConfig.yaml文件（直接运行会报错并生成一个默认配置文件）

```yaml
ManagerServerIP: 127.0.0.1
ManagerServerPort: 28080
VirtualAddress: APIService
ServiceName: APIService1
ServiceIP: 127.0.0.1
ServicePort: 5000
Description: APIService1 is a test
```