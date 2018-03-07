using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour {

    [SerializeField]
    public float walkSpeed = 10f;
    [SerializeField]
    public float runSpeed = 15;
    public float crouchSpeed = 4f;
    [SerializeField]
    public float jumpSpeed = 8f;
    public float gravity = 20f;
    public LayerMask groundLayer;


    [SerializeField] private FPSMouseLook fpsMouseLook;

    private Transform fpsView;
    private Transform fpsCam;
    private Vector3 fpsViewRotation;


    private float speed;
    private bool isGrounded;
    private bool isMoving;
    private bool isCrouching;
    private bool isJumping;
    private float antiBumpFactor = 0.75f;
    private bool limitDiagonalSpeed = true;


    private CharacterController charController;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 moveInput = Vector3.zero;

    private Camera _camera;

    private float rayDistance;
    private float defaultControllerHeight;
    private Vector3 defaultCamPos;
    private float camHeight;

    private FPSAnimController animController;

    [SerializeField]
    private WeaponManager _weaponManager;
    private FPSWeapon _currentWeapon;
    
    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private WeaponManager _weaponHandManager;
    private FPSHandWeapon _currentHandWeapon;

    // Use this for initialization
    void Start () {
        fpsView = transform.Find("FPSView").transform;
        charController = transform.GetComponent<CharacterController>();
        animController = transform.GetComponent<FPSAnimController>();
        _camera = Camera.main;
        fpsMouseLook.Init(transform, _camera.transform);

        rayDistance = charController.height * 0.5f + charController.radius;
        defaultControllerHeight = charController.height;
        defaultCamPos = fpsView.localPosition;

        speed = walkSpeed;

        isMoving = false;
        isCrouching = false;

        // get first weapon
        var weaponGO = _weaponManager.weapons[0];
        weaponGO.SetActive(true);
        _currentWeapon = weaponGO.GetComponent<FPSWeapon>();

        var weaponHandGO = _weaponHandManager.weapons[0];
        weaponHandGO.SetActive(true);
        _currentHandWeapon = weaponHandGO.GetComponent<FPSHandWeapon>();
    }
	
	// Update is called once per frame
	void Update () {
        RotateView();

        if(isGrounded)
        {
            PlayerCrouchingAndSprinting();
            PlayerJump();      
        }
        SelectWeapon();
    }


    void FixedUpdate()
    {
        PlayerMovement();
        fpsMouseLook.UpdateCursorLock();
    }

    void LateUpdate()
    {
        if(animController!= null)
        {
            LookToGunDirection();
        }
    }

    void LookToGunDirection()
    {
        // ik look to mouse look
        var point = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _camera.nearClipPlane + 0.5f));
        if (point != Vector3.zero)
        {
            animController.UpdateLook(point, offset);
        }
    }


    public void PlayerMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        moveInput = new Vector3(h, 0, v);
        if(moveInput.sqrMagnitude > 1)
        {
            moveInput.Normalize();
        }

        var vel = transform.forward * moveInput.z + transform.right * moveInput.x;

        // rotate
        //fpsViewRotation = Vector3.Lerp(fpsViewRotation, Vector3.zero, Time.fixedDeltaTime * 5f);
        // fpsView.localEulerAngles = fpsViewRotation;
        moveDirection.x = vel.x * speed;
        moveDirection.z = vel.z * speed;
        if (isGrounded)
        {
            if(!isJumping)
            {
               
            }
           
        }else
        {
            moveDirection.y -= gravity * Time.fixedDeltaTime;
        }

    
      

        isGrounded = (charController.Move(moveDirection * Time.fixedDeltaTime) & CollisionFlags.Below) != 0;
    
        isMoving = charController.velocity.magnitude > 0.15f;

    
        HandleAnimations();
    }


    void PlayerJump()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(isCrouching)
            {
                if(CanGetUp())
                {
                    isCrouching = false;
                   

                    StopCoroutine(MoveCameraCrouch());
                    StartCoroutine(MoveCameraCrouch());
                    animController.PlayerCrouch(isCrouching);
                }
            }else
            {
             
                moveDirection.y = jumpSpeed;
                print("jump: "+moveDirection);
                isJumping = true;
            }
        }
    }


    void PlayerCrouchingAndSprinting()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if(!isCrouching)
            {
                isCrouching = true;
               
            }else
            {
                if(CanGetUp())
                {
                    isCrouching = false;
                }
            }

            StopCoroutine(MoveCameraCrouch());
            StartCoroutine(MoveCameraCrouch());
        }

        if (isCrouching)
        {
            speed = crouchSpeed;
        }else
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }else
            {
                speed = walkSpeed;
            }
        }
        animController.PlayerCrouch(isCrouching);
    }

    bool CanGetUp()
    {
        Ray ray = new Ray(transform.position, transform.up);
        RaycastHit groundHit;
        if(Physics.SphereCast(ray, charController.radius + 0.05f, out groundHit, rayDistance, groundLayer))
        {
            var dist = Vector3.Distance(transform.position, groundHit.point);
            if (dist < 2.3f)
                return false;
        }
        return true;
    }

    IEnumerator MoveCameraCrouch()
    {
        var divideHeight = 1.5f;
        charController.height = isCrouching ? defaultControllerHeight / divideHeight : defaultControllerHeight;
        charController.center = new Vector3(0, charController.height / 2f, 0);

        camHeight = isCrouching ? defaultCamPos.y / divideHeight : defaultCamPos.y;

        while(Mathf.Abs(camHeight - fpsView.localPosition.y) > 0.01f)
        {
            fpsView.localPosition = Vector3.Lerp(fpsView.localPosition, 
                new Vector3(defaultCamPos.x, camHeight, defaultCamPos.z), Time.deltaTime * 11f);

            yield return null;
        }
    }


    void HandleAnimations()
    {
        animController.Movement(charController.velocity.magnitude);
        animController.PlayerJump(charController.velocity.y);
        if(isCrouching && charController.velocity.magnitude > 0)
        {
            animController.PlayerCrouchWalk(charController.velocity.magnitude);
        }

        // shoot
        if (_currentWeapon != null)
        {
            if (_currentWeapon.CurrentWeaponType == FPSWeaponBase.WeaponType.Pistol)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    Shoot();
                }
            }
            else if (_currentWeapon.CurrentWeaponType == FPSWeaponBase.WeaponType.Rifle)
            {
                if (Input.GetMouseButton(0))
                {
                    Shoot();
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
          
            animController.Reload();
            _currentHandWeapon.Reload();
        }

        if(Input.GetMouseButtonDown(1))
        {
            _currentHandWeapon.AddSilencer();
        }
    }


    private void Shoot()
    {
        var isFired = _currentWeapon.Fire();
        if(isFired)
        {
            if (isCrouching)
            {
                animController.Shoot(false);
            }
            else
            {
                animController.Shoot(true);
            }
            _currentHandWeapon.Shoot();
            
        }
        
    }


    void SelectWeapon()
    {
        int changeWeapon = -1;
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeWeapon = 0;
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeWeapon = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changeWeapon = 2;
        }
        if (changeWeapon != -1)
        {
            var weapons = _weaponManager.weapons;
            var handWeapons = _weaponHandManager.weapons;
            if (changeWeapon <= weapons.Length - 1)
            {
                if(!weapons[changeWeapon].activeInHierarchy)
                {
                    // 3d view
                    for (int i = 0; i < weapons.Length; i++)
                    {
                        weapons[i].SetActive(false);
                    }
                    _currentWeapon = null;
                    weapons[changeWeapon].SetActive(true);
                    _currentWeapon = weapons[changeWeapon].GetComponent<FPSWeapon>();

                    // local user view
                    for (int i = 0; i < handWeapons.Length; i++)
                    {
                        handWeapons[i].SetActive(false);
                    }

                    _currentHandWeapon = null;
                    handWeapons[changeWeapon].SetActive(true);
                    _currentHandWeapon = handWeapons[changeWeapon].GetComponent<FPSHandWeapon>();

                    animController.ChangeAnimationController(_currentWeapon.CurrentWeaponType == FPSWeaponBase.WeaponType.Pistol);
                }
            }


        }
    }
    private void RotateView()
    {
        fpsMouseLook.LookRotation(transform, _camera.transform);
    }
}
