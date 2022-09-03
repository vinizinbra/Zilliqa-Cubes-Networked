// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerManager.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in PUN Basics Tutorial to deal with the networked player instance
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cube : MonoBehaviourPunCallbacks, IPunObservable
{
    private int materialIndex = 0;
    private bool _rotating = false;
    [SerializeField]
    private MeshRenderer _renderer;
    public int MaterialIndex
    {
        get => materialIndex;
        set
        {
            if (materialIndex != value)
            {
                materialIndex = value;
                UpdateMaterial();
            }
        }
    }

    public MaterialConfig config;
    
    void UpdateMaterial()
    {
        _renderer.material = config.materials[materialIndex];
    }
    
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateMaterial();
    }

    public override void OnDisable()
	{
		base.OnDisable ();

		UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
	}
    
    public void Update()
    {
        if (photonView.IsMine)
        {
            this.ProcessInputs();
        }

        if (_rotating)
        {
            transform.Rotate(Vector3.up*15);
        }
    }

    void CalledOnLevelWasLoaded(int level)
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
	{
		this.CalledOnLevelWasLoaded(scene.buildIndex);
	}

    void ProcessInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int nextTextureIndex = materialIndex + 1;
            MaterialIndex = (nextTextureIndex) % config.materials.Length;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _rotating = !_rotating;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this._rotating);
            stream.SendNext(this.materialIndex);
        }
        else
        {
            this._rotating = (bool)stream.ReceiveNext();
            this.MaterialIndex = (int)stream.ReceiveNext();
        }
    }
}
