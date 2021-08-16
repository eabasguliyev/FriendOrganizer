using System;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Autofac;
using Autofac.Core;
using FriendOrganizer.DataAccess;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.ViewModels;
using FriendOrganizer.UI.Views.Services;
using Prism.Events;

namespace FriendOrganizer.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<FriendOrganizerDbContext>().AsSelf();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();

            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            builder.RegisterType<FriendDetailViewModel>().Keyed<IDetailViewModel>(nameof(FriendDetailViewModel));
            builder.RegisterType<MeetingDetailViewModel>().Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));


            builder.RegisterType<LookupDataService>().AsImplementedInterfaces();
            builder.RegisterType<FriendRepository>().As<IFriendRepository>();
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();


            //builder.Register(async context =>
            //{
            //    return await Task.Factory.StartNew(() =>
            //        new Func<FriendOrganizerDbContext>(() => new FriendOrganizerDbContext()));
            //});

            //builder.Register(async context =>
            //{
            //    return await Task.Factory.StartNew(() => new FriendOrganizerDbContext());
            //});

            //builder.Register(async context =>
            //{
            //    return await Task.Run(() =>
            //    {
            //        return new Func<Task<FriendOrganizerDbContext>>(async () =>
            //        {
            //            return await Task.Run(() => new FriendOrganizerDbContext());
            //        });
            //    });
            //}).AsSelf();

            //builder.RegisterInstance(new AsyncRegistration<FriendOrganizerDbContext>(async context =>
            //{
            //    return await Task.Run(() => new FriendOrganizerDbContext());
            //})).AsSelf();

            //builder.Register<FriendOrganizerDbContext>(context =>
            //{
            //    var asyncRegistration = context.Resolve<AsyncRegistration<FriendOrganizerDbContext>>();
            //    if (!asyncRegistration.Resolved)
            //        throw new DependencyResolutionException(
            //            $"Async component {typeof(FriendOrganizerDbContext).Name} has not been resolved");

            //    return asyncRegistration.Value;
            //});

            //builder.Register(asycontext =>
            //{
            //    return new Task<FriendOrganizerDbContext>(async () =>
            //    {
            //        return await Task.Run(() => new FriendOrganizerDbContext());
            //    });
            //}).AsSelf();

            return builder.Build();
        }
    }
}