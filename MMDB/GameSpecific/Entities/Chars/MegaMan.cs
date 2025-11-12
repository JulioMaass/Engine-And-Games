using Engine.ECS.Components.ControlHandling.Conditions;
using Engine.ECS.Components.ControlHandling.SecondaryStates;
using Engine.ECS.Components.ControlHandling.States;
using Engine.ECS.Components.ShootingHandling;
using Engine.ECS.Entities.EntityCreation;
using Engine.Types;
using MMDB.GameSpecific.States;
using MMDB.GameSpecific.States.Player;
using MMDB.GameSpecific.Weapons;
using System.Collections.Generic;
using System.Linq;
using StateFall = MMDB.GameSpecific.States.Player.StateFall;

namespace MMDB.GameSpecific.Entities.Chars;

public class MegaMan : Entity
{
    public MegaMan()
    {
        EntityKind = EntityKind.Player;

        // Basic, Sprite, EntityKind
        AddBasicComponents();
        AddSprite("MegaMan", 34, 39, 14, 14);
        AddMmdbPlayerComponents();
        AddMegaManComponents();

        // Speeds
        AddMoveSpeed(1.5f);
        AddJumpSpeed(5.05f);
        AddDashSpeed(2.5f);

        // Weapons
        WeaponManager.Weapons.Add(new WeaponBuster(this));
        WeaponManager.Weapons.Add(new WeaponChain(this));
        WeaponManager.Weapons.Add(new WeaponGas(this));
        WeaponManager.Weapons.Add(new WeaponPolice(this));
        WeaponManager.Weapons.Add(new WeaponGhost(this));
        WeaponManager.Weapons.Add(new WeaponOcean(this));
        WeaponManager.Weapons.Add(new WeaponVacuum(this));
        WeaponManager.Weapons.Add(new WeaponJungle(this));
        WeaponManager.Weapons.Add(new WeaponRemote(this));
        WeaponManager.CurrentWeapon = WeaponManager.Weapons.FirstOrDefault();
        ChargeManager = new(this);
        ChargeManager.ChargeTierFrames = [40, 80];
        AddPalette("MMPalette");

        // Control components
        // TODO - ARCHITECTURE - MMDB: Simplify state creation or move elsewhere
        // TODO: Use the new AiControl pattern (EngineStates: teleport/hurt; CommandStates: climb/jump/slide/stepwalk; AutomaticStates: fall/idle)
        // Automatic states
        var stateTeleport = NewState(new StateTeleport(180), 19);
        // Air states
        var stateHurt = NewState(new StateHurt(0.5f, 20), 12);
        var stateClimb = NewStateWithTimedPattern(new StateClimb(1.4f), (13, 6), (14, 3), (15, 3));
        var stateJump = NewState(new StateJump(), 10);
        var stateFall = NewState(new StateFall(), 10);
        // Ground states
        var stateSlide = NewState(new StateSlide(25), 11);
        stateSlide.SetCustomHitbox(new IntRectangle(8, 0, 16, 16));
        var stateStepWalk = NewStateWithStartUp(new StateStepWalk(), 2, 4, 8, 6, 1);
        var stateIdle = NewStateWithTimedPattern(new StateIdle(), (0, 90), (17, 8), (0, 120), (17, 8));
        AddStateManager();
        var stateList = new List<State> { stateTeleport, stateHurt, stateClimb, stateJump, stateFall, stateSlide, stateStepWalk, stateIdle };
        StateManager.AutomaticStatesList.AddRange(stateList);

        // Secondary states
        StateManager.AddSecondaryState<SecondaryStateShoot>(state =>
        {
            state.SpriteIdOffset = 20;
            state.Duration = 16;
        });
        StateManager.AddSecondaryState<SecondaryStateNone>(state =>
        {
            state.SpriteIdOffset = 0;
        });

        DamageTaker.SetInvincibilityFrames(60);

        // Set initial state
        PlayerControl.StartTeleporting.TurnOn();

        // Transition controller
        TransitionController = new(this);
        var upCondition = new ConditionState(stateClimb);
        upCondition.Owner = this;
        TransitionController.UpCondition = upCondition;
        var sidesCondition = new ConditionStateNot(stateHurt);
        sidesCondition.Owner = this;
        TransitionController.SidesCondition = sidesCondition;
    }
}