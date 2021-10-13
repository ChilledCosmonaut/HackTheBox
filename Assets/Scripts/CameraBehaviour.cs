using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject Camera;
    public GameObject leftSide;
    public GameObject frontSide;
    public GameObject rightSide;
    public GameObject backSide;
    private GameObject[] _cameraPosition = new GameObject[4];
    private float _zoom;
    public float zoomScale;
    public Vector3 targetGlobalPosition;
    private Quaternion _targetRotation;
    public float smoothness = 250;
    public int currentPosition = 0; //Merkt sich die position per Index
    private Vector2 _position;
    public List<GameObject> healthBars;
    public int speed;
    private bool _rotated;
    private bool _targetReached = true;
    private GameController _controller;

    void Start()
    {
        _targetRotation = backSide.transform.rotation;
        currentPosition = 0;
        _cameraPosition[0] = backSide;
        _cameraPosition[1] = leftSide;
        _cameraPosition[2] = frontSide;
        _cameraPosition[3] = rightSide;
        GetHealthBars();
    }

    public void GetHealthBars()
    {
        healthBars.Clear();
        GameObject[] cache = GameObject.FindGameObjectsWithTag("UnitInfo");
        foreach (GameObject healthBar in cache)
        {
            healthBars.Add(healthBar);
        }
    }

    void Update()
    {
        _position.x = Input.GetAxis("Horizontal") * speed;
        _position.y = Input.GetAxis("Vertical") * speed;
        _position.x *= Time.deltaTime;
        _position.y *= Time.deltaTime; 
        _zoom = Input.GetAxis("Mouse ScrollWheel") * zoomScale;
        _zoom *= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LeftRotate();
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            RightRotate();
        }
        
        Camera.transform.rotation = Quaternion.Lerp(Camera.transform.rotation, _targetRotation, Time.deltaTime * smoothness);

        if (!_rotated)
        {
            if (Math.Abs(Vector3.Distance(_cameraPosition[currentPosition].transform.position, Camera.transform.position)) >= 0.5f)
            {
                Camera.transform.position = Vector3.Lerp(Camera.transform.position, _cameraPosition[currentPosition].transform.position, Time.deltaTime * smoothness);
            }
            else
            {
                _rotated = true;
            }
        }
        
        if (!_targetReached)
        {
            if (Math.Abs(Vector3.Distance(targetGlobalPosition, gameObject.transform.position)) >= 0.5f)
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetGlobalPosition, Time.deltaTime * smoothness);;
            }
            else
            {
                _targetReached = true;
            }
        }

        try
        {
            foreach (GameObject healthBar in healthBars)
            {
                healthBar.transform.rotation = Camera.transform.rotation;
            }
        }
        catch (Exception e)
        {
            GetHealthBars();
            
            foreach (GameObject healthBar in healthBars)
            {
                healthBar.transform.rotation = Camera.transform.rotation;
            }
        }

        switch (currentPosition)
        {
            case 0:
                transform.Translate(_position.x, 0, _position.y);
                break;
            case 1:
                transform.Translate(_position.y, 0,-_position.x );
                break;
            case 2:
                transform.Translate(-_position.x, 0, -_position.y);
                break;
            case 3:
                transform.Translate(-_position.y, 0,_position.x );
                break;
        }

        if (transform.position.y >= 0 || _zoom <= 0)
        {
            switch (currentPosition)
            {
                case 0:
                    transform.Translate(0, -_zoom, _zoom);
                    break;
                case 1:
                    transform.Translate(_zoom, -_zoom,0);
                    break;
                case 2:
                    transform.Translate(0, -_zoom, -_zoom);
                    break;
                case 3:
                    transform.Translate(-_zoom, -_zoom,0);
                    break;
            }
        }
    }

    public void LeftRotate()// Drehung der Kamera nach links
    {
        switch (currentPosition)//Es wird hier immer nur die ROtation geändert nicht die Position, kann sich noch gameplay wegen ändern
        {
            case 0:
                _targetRotation = leftSide.transform.rotation;
                currentPosition = 1;
                break;
            case 1:
                _targetRotation = frontSide.transform.rotation;
                currentPosition = 2;
                break;
            case 2:
                _targetRotation = rightSide.transform.rotation;
                currentPosition = 3;
                break;
            case 3:
                _targetRotation = backSide.transform.rotation;
                currentPosition = 0;
                break;
        }
        _rotated = false;
    }

    public void RightRotate()// Drehung der Kamera nach rechts
    {
        switch (currentPosition)
        {
            case 0:
                _targetRotation = rightSide.transform.rotation;
                currentPosition = 3;
                break;
            case 1:
                _targetRotation = backSide.transform.rotation;
                currentPosition = 0;
                break;
            case 2:
                _targetRotation = leftSide.transform.rotation;
                currentPosition = 1;
                break;
            case 3:
                _targetRotation = frontSide.transform.rotation;
                currentPosition = 2;
                break;
        }
        _rotated = false;
    }

    public void CameraSwitch(GameObject unit)
    {
        targetGlobalPosition = unit.transform.position;
        _targetReached = false;
    }
}
