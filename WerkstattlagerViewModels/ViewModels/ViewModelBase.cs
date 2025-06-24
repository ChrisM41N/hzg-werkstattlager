using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkstattlagerAPI;

namespace WerkstattlagerViewLogic.ViewModels;

public abstract class ViewModelBase(ILogger? logger, IDbContextFactory<InventoryContext> dbContextFactory) : ObservableObject
{
    protected readonly ILogger? Logger = logger;
    protected readonly IDbContextFactory<InventoryContext> DbContextFactory = dbContextFactory;
}
