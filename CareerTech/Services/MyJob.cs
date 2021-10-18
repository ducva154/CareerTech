using CareerTech.Models;
using CareerTech.Services.Implement;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;
namespace CareerTech.Services
{
    public class MyJob : IJob
    {
        private readonly IPartnerManagementService<PartnerManagementService> _partnerManagementService;
        public MyJob(IPartnerManagementService<PartnerManagementService> partnerManagementService)
        {
            _partnerManagementService = partnerManagementService;
        }
        public MyJob()
        {
            _partnerManagementService = new PartnerManagementService(new ApplicationDbContext());
        }
        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() =>
            {
                var partnerService = _partnerManagementService.getPartnersWithService();
                partnerService.ForEach(async ps =>
                {
                    if (ps.endDate <= DateTime.Now && ps.Status.Equals("Approved"))
                    {
                        var ExpiredPartner = _partnerManagementService.getPartnerByID(ps.CompanyID);
                        await _partnerManagementService.RejectPartner(ps.CompanyID);
                    }
                });

            });
            return task;
        }
    }

    public class JobSchedule
    {
        public static async Task Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();
            IJobDetail job = JobBuilder.Create<MyJob>().Build();
            ITrigger trigger = TriggerBuilder.Create().StartNow()
              .WithSimpleSchedule(
                s => s.WithIntervalInSeconds(1).RepeatForever()).Build();
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
