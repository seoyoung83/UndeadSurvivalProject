using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleOfPentagonFlame : MonoBehaviour , IPlayerBullet
{
    int skillLevel;

    AudioSource skillAudio;

    [SerializeField] GameObject flameObject;
    [SerializeField] GameObject bottleObject;

    Vector3 shootVect;
    Vector3 muzzleVect;

    bool isBrokenBottle = false; //TRUE-> FIRE

    float moveTime;
    float moveSpeed = 2.5f;
    float rotateSpeed = 270f;
    float rotateZ;

    float duration;
    float checkingTimer = 0;

    private void OnEnable()
    {
        flameObject.SetActive(false);
        bottleObject.SetActive(true);
    }

    public void UpdateSkillLevel(int _level)
    {
        skillLevel = _level;

        flameObject.GetComponent<PentagonFlame>().GetSkillLevel(skillLevel);
    }

    private void Update()
    {
        if (bottleObject.activeInHierarchy)
        {
            rotateZ -= Time.deltaTime * rotateSpeed;
            bottleObject.transform.localRotation = Quaternion.Euler(0, 0, rotateZ);

            if (shootVect != null)
                Fire(shootVect);
        }

        if (isBrokenBottle)
        {
            checkingTimer += Time.deltaTime;

            if (checkingTimer > duration)
            {
                checkingTimer = 0f;

                gameObject.SetActive(false);
                flameObject.SetActive(false);
            }
        }
    }

    public void Fire(Vector3 _shootVect)
    {
        moveTime += Time.deltaTime * moveSpeed;
        transform.position = Vector3.MoveTowards(muzzleVect, _shootVect, moveTime);

        float distanceToTarget = (_shootVect - transform.position).magnitude;

        if (distanceToTarget < 0.03f)
        {
            bottleObject.SetActive(false);

            flameObject.SetActive(true);

            skillAudio.Play();

            isBrokenBottle = true;

            moveTime = 0;

            rotateZ = 0;
        }
    }

    public void UpdateSkillInfo(AudioSource _skillAudio , Vector3 _target, float _duration)
    {
        skillAudio = _skillAudio;

        checkingTimer = 0f;

        rotateZ = 0;

        duration = _duration;

        shootVect = _target;

        isBrokenBottle = false;
    }

    public void SetMuzzleTransform(Transform muzzleTransform)
    {
        flameObject.transform.localPosition = Vector3.zero;
        bottleObject.transform.localPosition = Vector3.zero;

        bottleObject.transform.localRotation = Quaternion.identity;

        muzzleVect = muzzleTransform.position;
    }
}
