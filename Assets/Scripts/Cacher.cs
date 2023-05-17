using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

//TODO : �ֿ� Ŭ�������� ȣ�� Ÿ�̹��� ��Ȯ�ϰ� �������� ���� ���� ����.
//       �ʱ�ȭ ������ ��Ȯ�ϰ� �����ϰ�, �ʱ�ȭ�� �Ϸ�Ǿ����� Ȯ���ϴ� ����� ã�ƾ���.
//       �� ���� Ŭ������ ���� Getter�� FindObjectOfType ȣ���� �ּ�ȭ �ϱ� ���� �ӽù���.
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