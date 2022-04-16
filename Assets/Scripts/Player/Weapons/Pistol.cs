namespace Player.Weapons
{
    public class Pistol : Weapon
    {
        public override void SwapWeapon(int amount)
        {
            playerManager.WeaponType = WeaponType.Pistol;

            //playerManager.GetWeapon();
            //playerManager.SetWeapon(WeaponType.Pistol);
            playerManager.ItemPlayer.amount = amount;

            //PlayerManager.Instance.WeaponType = WeaponType.Pistol;
            //
            //PlayerManager.Instance.GetWeapon();
            //PlayerManager.Instance.SetWeapon(WeaponType.Pistol);
            //PlayerManager.Instance.ItemPlayer.Amount = amount;
        }
    }
}