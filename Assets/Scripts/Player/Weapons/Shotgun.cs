namespace Player.Weapons
{
    public class Shotgun : Weapon
    {
        public override void SwapWeapon(int amount)
        {
            PlayerManager.Instance.WeaponType = WeaponType.Shotgun;
            PlayerManager.Instance.GetWeapon();
            PlayerManager.Instance.SetWeapon(WeaponType.Shotgun);
            PlayerManager.Instance.ItemPlayer.Amount = amount;
        }
    }
}