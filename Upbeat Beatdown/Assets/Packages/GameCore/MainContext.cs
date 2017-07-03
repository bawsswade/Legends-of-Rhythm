using UnityEngine;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using strange.extensions.injector.api;
using strange.extensions.signal.impl;
using System.Linq;

public class MainContext : MVCSContext
{
    public MainContextView MainCtx;

    public MainContext(MonoBehaviour ctxView, bool autoMap)
        : base(ctxView, autoMap)
    {
        MainCtx = (MainContextView)ctxView;
        MainCtx.Packages = MainCtx.GetComponentsInChildren<StrangePackage>().ToList();
    }

    protected override void addCoreComponents()
    {
        base.addCoreComponents();
        injectionBinder.Unbind<ICommandBinder>();
        injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();

        injectionBinder.Bind<ICrossContextInjectionBinder>().ToValue(injectionBinder);
    }

    protected override void mapBindings()
    {
        //Bind start Game Singal that dispatches at launch.
        commandBinder.Bind<StartGameCoreSignal>();

        foreach(var pack in MainCtx.Packages)
        {
            pack.MapBindings(commandBinder, injectionBinder, mediationBinder);
        }
    }

    protected override void postBindings()
    {
        foreach (var pack in MainCtx.Packages)
        {
            pack.PostBindings(commandBinder, injectionBinder, mediationBinder);
        }
    }

    public override void Launch()
    {
        foreach (var pack in MainCtx.Packages)
        {
            pack.Launch(injectionBinder);
        }
        injectionBinder.GetInstance<StartGameCoreSignal>().Dispatch();
    }
}
public class StartGameCoreSignal : Signal { }


