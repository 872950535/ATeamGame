using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DG.Tweening;
public class GunController : MonoBehaviour
{
	[Header("===Gun Setting===")]
	public Transform weaponHold;
	public Gun startingGun;
	public Gun equippedGun;

	private int remainBullet;
	private int maxBullet;
	[Header("===Bullet Setting===")]
	public Text bulletText;
	public Text maxBulletText;

	bool can_shot = true;
	[Header("===shell Pool===")]
	public Shell shellPrefab;
	public Transform ShellPositon;
	public ShellPool shellPool;
	public Transform shellPoolTransfrom;
	void Start() {
		if (startingGun != null) {
			EquipGun(startingGun);
			remainBullet = maxBullet;
		}
	}

	void Update()
    {
		bulletText.text = remainBullet.ToString();
    }
	public void EquipGun(Gun gunToEquip) {
		if (equippedGun != null) {
			Destroy(equippedGun.gameObject);
		}
		equippedGun = Instantiate (gunToEquip, weaponHold.position,weaponHold.rotation) as Gun;
		equippedGun.transform.parent = weaponHold;
		maxBullet = equippedGun.bulletNumber;
		equippedGun.OnShoot.AddListener(ReduceBullet);

		maxBulletText.text ="/"+maxBullet.ToString();
	}

	public void Shoot() 
	{
		if (equippedGun != null&&remainBullet>0&&can_shot)
		{
			equippedGun.Shoot();
			equippedGun.fireEffect.SetActive(true);
		}
		else if(remainBullet<=0)
        {
			equippedGun.fireEffect.SetActive(false);
			Reload();
        }
	}

    public void Reload()
    {
		if (can_shot)
		{
			can_shot = false;
			AudioManager.instance.Relod(this.transform.position);
			Invoke("Reloading", 2f);
		}
    }

    public void ReduceBullet()
    {
		Shell newShell = shellPool.GetInstance();
		newShell.Reset();
		newShell.transform.position = ShellPositon.position;
		newShell.transform.rotation = ShellPositon.rotation;
		newShell.transform.SetParent(shellPoolTransfrom);
		newShell.OnShellDestory.AddListener(ReturnShellToPool);
		AudioManager.instance.Shoot(this.transform.position);
		remainBullet--;
    }
	public void ReturnShellToPool(Shell shell)
    {
		shell.OnShellDestory.RemoveAllListeners();
		shellPool.ReturnInstance(shell,shellPoolTransfrom);
    }
	public void Reloading()
    {
		remainBullet = maxBullet;
		can_shot = true;
	}
}