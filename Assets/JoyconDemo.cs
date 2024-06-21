using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalmanFilter
{
    private float Q; // process noise covariance
    private float R; // measurement noise covariance
    private float P; // estimation error covariance
    private float X; // value

    public KalmanFilter(float q, float r, float p, float initial_value)
    {
        Q = q;
        R = r;
        P = p;
        X = initial_value;
    }

    public float Update(float measurement)
    {
        // Prediction update
        P = P + Q;

        // Measurement update
        float K = P / (P + R); // Kalman gain
        X = X + K * (measurement - X);
        P = (1 - K) * P;

        return X;
    }
}

public class JoyconDemo : MonoBehaviour
{

    private List<Joycon> joycons;

    // Values made available via Unity
    public float[] stick;
    public Vector3 gyro;
    public Vector3 accel;
    public int jc_ind = 0;
    public Quaternion orientation;

    private Vector3 position;
    private Vector3 velocity;

    private KalmanFilter kalmanFilterX;
    private KalmanFilter kalmanFilterY;
    private KalmanFilter kalmanFilterZ;

    private Vector3 accelBias;
    private bool isCalibrated = false;

    void Start()
    {
        gyro = new Vector3(0, 0, 0);
        accel = new Vector3(0, 0, 0);
        position = new Vector3(0, 0, 0);
        velocity = new Vector3(0, 0, 0);

        // Initialize Kalman filters for each axis
        kalmanFilterX = new KalmanFilter(0.001f, 0.1f, 1.0f, 0);
        kalmanFilterY = new KalmanFilter(0.001f, 0.1f, 1.0f, 0);
        kalmanFilterZ = new KalmanFilter(0.001f, 0.1f, 1.0f, 0);

        // Initialize accelerometer bias
        accelBias = Vector3.zero;

        // Get the public Joycon array attached to the JoyconManager in scene
        joycons = JoyconManager.Instance.j;
        if (joycons.Count < jc_ind + 1)
        {
            Destroy(gameObject);
        }

        // Calibrate the accelerometer to find the bias
        StartCoroutine(CalibrateAccelerometer());
    }

    IEnumerator CalibrateAccelerometer()
    {
        yield return new WaitForSeconds(5); // Wait for 5 seconds before starting calibration

        Vector3 accelSum = Vector3.zero;
        int samples = 0;

        float startTime = Time.time;
        while (Time.time < startTime + 5) // Collect data for the next 5 seconds
        {
            accelSum += joycons[jc_ind].GetAccel();
            samples++;
            yield return null; // Wait for next frame
        }

        accelBias = accelSum / samples;
        accelBias = new Vector3(
            Mathf.Floor(accelBias.x * 100f) / 100f,
            Mathf.Floor(accelBias.y * 100f) / 100f,
            Mathf.Floor(accelBias.z * 100f) / 100f
        );

        isCalibrated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (joycons.Count > 0 && isCalibrated)
        {
            Joycon j = joycons[jc_ind];

            if (j.GetButtonDown(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 pressed");
                Debug.Log(string.Format("Stick x: {0:N} Stick y: {1:N}", j.GetStick()[0], j.GetStick()[1]));
                j.Recenter();
            }

            if (j.GetButtonUp(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 released");
            }

            if (j.GetButton(Joycon.Button.SHOULDER_2))
            {
                Debug.Log("Shoulder button 2 held");
            }

            if (j.GetButtonDown(Joycon.Button.DPAD_DOWN))
            {
                Debug.Log("Rumble");
                j.SetRumble(160, 320, 0.6f, 200);
            }

            stick = j.GetStick();
            gyro = j.GetGyro();
            accel = j.GetAccel();
            orientation = j.GetVector();

            // Apply Kalman filter to accelerometer data after removing bias
            float dt = Time.deltaTime; // Time elapsed since last frame
            Vector3 filteredAccel = new Vector3(
                kalmanFilterX.Update(Mathf.Floor((accel.x - accelBias.x) * 100f) / 100f),
                kalmanFilterY.Update(Mathf.Floor((accel.y - accelBias.y) * 100f) / 100f),
                kalmanFilterZ.Update(Mathf.Floor((accel.z - accelBias.z) * 100f) / 100f)
            );

            // Update velocity and position
            velocity += filteredAccel * dt;
            position += velocity * dt;

            // Update the object's position
            gameObject.transform.position = position;
            gameObject.transform.rotation = orientation;

            if (j.GetButton(Joycon.Button.DPAD_UP))
            {
                gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material.color = Color.blue;
            }
        }
    }
}
