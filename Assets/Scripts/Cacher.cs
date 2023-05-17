using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

//TODO : 주요 클래스들의 호출 타이밍을 정확하게 조절하지 못해 생긴 문제.
//       초기화 순서를 정확하게 조절하고, 초기화가 완료되었는지 확인하는 방법을 찾아야함.
//       각 동적 클래스에 들어가는 Getter의 FindObjectOfType 호출을 최소화 하기 위한 임시방편.
public static class Cacher
{ 
    static CargoManager cargo;
    static UIManager ui;
    static InputManager input;
    static ULDManager uld;
    static DataManager data;

    static Cacher()
    {
        cargo = GameObject.FindObjectOfType<CargoManager>();
        ui = GameObject.FindObjectOfType<UIManager>();
        input = GameObject.FindObjectOfType<InputManager>();
        uld = GameObject.FindObjectOfType<ULDManager>();
        data = GameObject.FindObjectOfType<DataManager>();
    }
    public static CargoManager cargoManager
    {
        get
        {
            return cargo;
        }
    }

    public static UIManager uiManager
    {
        get
        {
            return ui;
        }
    }

    public static InputManager inputManager
    {
        get
        {
            return input;
        }
    }

    public static ULDManager uldManager
    {
        get
        {
            return uld;
        }
    }

    public static DataManager dataManager
    {
        get
        {
            return data;
        }
    }
}