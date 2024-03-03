using System;
using System.Collections.Generic;
using UnityEngine;

// ќбъ€вление класса PlayerData, который будет сериализован (может быть сохранен и загружен).
[Serializable]
public class PlayerData
{

    // —писок (коллекци€) предметов, наход€щихс€ в инвентаре игрока.
    public List<FormData> tasks;
}
