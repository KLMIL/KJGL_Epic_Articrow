public class Define
{
    #region Scene
    // 씬 타입(이름)
    public enum SceneType
    {
        None,
        TitleScene,
        InGameScene,
        EndingScene
    }
    #endregion

    #region Sound
    // Resources 내의 BGM, SFX 폴더에 같은 이름으로 둬야함
    public enum BGM
    {
        None,
        TitleScene,
        InGameScene,
        EndingScene
    }
    public enum SFX
    {
        None,
        DoorOpen,
        DoorClose,
        DefaultAttack,
        CastDamageArea,
        CastExplosion,
        HitDefaultAttack,
        HitDamageArea,
        HitExplosion,
        HitGrab,
        Put,
        Pickup,
        sfx_slime_die // 250611 KWS - test for enemy sound
    }
    #endregion

    public enum KeyAction
    {
        Move,
        Interact,
        Inventroy,
        LeftHand,
        RightHand,
        Roll,
    }
}