namespace Player.Weapons
{
    public class Shotgun : Weapon
    {
        public override void SwapWeapon(int amount)
        {
            playerManager.WeaponType = WeaponType.Shotgun;
            playerManager.GetWeapon();
            playerManager.SetWeapon(WeaponType.Shotgun);
            playerManager.ItemPlayer.amount = amount;

            //PlayerManager.Instance.WeaponType = WeaponType.Shotgun;
            //PlayerManager.Instance.GetWeapon();
            //PlayerManager.Instance.SetWeapon(WeaponType.Shotgun);
            //PlayerManager.Instance.ItemPlayer.Amount = amount;
        }
    }
}