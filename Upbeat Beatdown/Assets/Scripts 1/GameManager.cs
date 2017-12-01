using UnityEngine;
using System.Collections;
using strange.extensions.command.api;
using strange.extensions.injector.api;
using strange.extensions.mediation.api;

public partial class GameManager : StrangePackage
{

    public GameData GameData;

    public override void MapBindings(ICommandBinder commandBinder, ICrossContextInjectionBinder injectionBinder, IMediationBinder mediationBinder)
    {
        //Game
        injectionBinder.Bind<GameManager>().ToValue(this).ToSingleton();
        injectionBinder.Bind<GameData>().ToValue(this.GameData);

        //player
        mediationBinder.Bind<PlayerMovementView>().To<PlayerMovementMediator>();
        mediationBinder.Bind<PlayerActionsView>().To<PlayerActionsMediator>();
        mediationBinder.Bind<AtkAssistView>().To<AtkAssistMediator>();
        mediationBinder.Bind<BeatIndicatorView>().To<BeatIndicatorMediator>();
        mediationBinder.Bind<CameraView>().To<CameraMediator>();
        mediationBinder.Bind<PlayerInputView>().To<PlayerInputMediator>();
        mediationBinder.Bind<BeatManagerView>().To<BeatManagerMediator>();
        mediationBinder.Bind<BossView>().To<BossMediator>();
        mediationBinder.Bind<AttackView>().To<AttackMediator>();
        mediationBinder.Bind<EnvBuildView>().To<EnvBuildMediator>();

        // player signals
        commandBinder.Bind<OnLeftAttackSignal>();
        commandBinder.Bind<OnRightAttackSignal>();
        commandBinder.Bind<OnDashSignal>();
        commandBinder.Bind<OnChargeSpecial>();
        commandBinder.Bind<OnLeftResetHit>();
        commandBinder.Bind<OnRightResetHit>();
        commandBinder.Bind<OnGainHealth>();
        commandBinder.Bind<OnChangeNoteType>();
        commandBinder.Bind<OnAttacking>();
        commandBinder.Bind<OnEnemyInRange>();
        // boss signals
        commandBinder.Bind<OnBassAttackSignal>();
        commandBinder.Bind<OnMelodyAttackSignal>();
        commandBinder.Bind<OnBossTakeDamage>();
        commandBinder.Bind<OnInstantAttackSignal>();
        
        //beatzzz signal
        commandBinder.Bind<OnLeftHit>();
        commandBinder.Bind<OnRightHit>();
        //commandBinder.Bind<OnBassBeat>();
        //commandBinder.Bind<OnMelodyBeat>();

        //managers  
        // main context view, window manager
        // manager data
    }
    public override void PostBindings(ICommandBinder commandBinder, ICrossContextInjectionBinder injectionBinder, IMediationBinder mediationBinder)
    {
    }

}
