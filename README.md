# Malenia Masterclass - Recreación del Combate Fase 1

**Proyecto patrocinado por:**

[![Escuela Musk](https://img.shields.io/badge/Patrocinado%20por-Escuela%20Musk-blue)](https://escuelamusk.com/videojuegos/)

*Escuela especializada en desarrollo de videojuegos - [escuelamusk.com/videojuegos](https://escuelamusk.com/videojuegos/)*

---

## 📚 Recursos de la Masterclass

Este repositorio fue creado para una **Masterclass en directo** sobre cómo reproducir el combate contra Malenia de Elden Ring en Unity. Aquí están todos los recursos mencionados durante la sesión:

### 🔗 Enlaces y Recursos Utilizados

**Proyecto Base:**
- [Proyecto original de Bruno Gottilieb - Souls Combat](https://github.com/BrunoGottlieb/Souls-Combat)

**Modelos 3D:**
- [Modelo Let Me Solo Her (PS1 Style)](https://crimsongcat.itch.io/let-me-solo-her-ps1-psx-elden-ring)
- [Modelo Malenia (PS1 Style)](https://crimsongcat.itch.io/malenia-ps1-psx/download/WgOeGmkjSAoiUF99jSx2veh3013JvLEX3a75CY0S)

**Assets de Unity:**
- [Shader de Agua Estilizada (URP)](https://assetstore.unity.com/packages/vfx/shaders/urp-stylized-water-shader-proto-series-187485)
- [Pack de Plantas y Flores Gratis](https://assetstore.unity.com/packages/2d/textures-materials/nature/grass-flowers-pack-free-138810)

**Herramientas de Desarrollo:**
- [Generador de Skyboxes con IA](https://ai.studio/apps/drive/1OF4J5xuJfUnpuTi4JMGePWyMveML6nLM)
- [Upscaler de imágenes con IA (Local)](https://upscayl.org/download)
- [Editor de modelos 3D online](https://products.aspose.app/3d/editor)
- [ActorCore - Rigging automático gratuito](https://actorcore.reallusion.com/auto-rig/accurig)
- [Mixamo - Animaciones y poses](https://www.mixamo.com/#/?genres=&page=2&query=pose&type=Motion%2CMotionPack)

**Documentación Técnica:**
- [Podcast: Sistema de combate de Malenia](https://g.co/gemini/share/e06a51bbd87c)
- [Wiki oficial - Ataques de Malenia](https://eldenring.wiki.fextralife.com/Malenia+Blade+of+Miquella)
- [Guía definitiva de ataques](https://claude.ai/public/artifacts/81674d84-ef01-4565-8ed4-a74435642a7f)
- [Análisis técnico completo del diseño de Malenia](https://docs.google.com/document/d/1YDgPfXgzqlteu4Kt9V0L7F4MWYtUqZKLAI9049m23iU/edit?usp=sharing)

---

## 🎮 Descripción del Proyecto

![Screenshot Principal](Assets/Screenshot/main.png)

Esta es una recreación técnica y educativa del icónico combate contra **Malenia, Blade of Miquella** (Fase 1) de Elden Ring, implementada en Unity. El proyecto fue desarrollado específicamente para demostrar técnicas avanzadas de programación de IA para boss fights y mecánicas de combate complejas.

### 🎯 Objetivos del Proyecto

- **Educativo**: Enseñar el desarrollo de IA compleja para boss fights
- **Técnico**: Implementar sistemas de combate avanzados
- **Recreativo**: Fidelidad al combate original de Elden Ring
- **Modular**: Código reutilizable para otros proyectos

---

## ✨ Características Principales

### 🤖 Sistema de IA Avanzado

La IA de Malenia utiliza una **Máquina de Estados Finitos (FSM)** con los siguientes componentes:

#### Estados Principales:
- **Idle/Stalk**: Movimiento neutro y evaluación de distancia
- **Melee_Aggressive**: Combos cuerpo a cuerpo agresivos
- **Ranged_Approach**: Ataques de acercamiento a media/larga distancia
- **WFD_Ready**: Estado especial para Waterfowl Dance

#### Lógica de Decisión:
```csharp
// Ejemplo del sistema de decisión basado en distancia
private void AI_Manager()
{
    if (distance >= farValue)
        action = "Move";
    else if (distance > nearValue && distance < farValue)
        action = "WaitForPlayer";
    else if (distance <= nearValue)
        action = "NearAttack";
}
```

### ⚔️ Sistema de Combate Completo

![Screenshot Combate](Assets/Screenshot/2.png)

#### Mecánicas Implementadas:
- **Lifesteal System**: Regeneración de vida al conectar ataques
- **Stance/Posture System**: Sistema de postura y breaks
- **Frame-Perfect Timing**: Ventanas precisas de evasión y contraataque
- **Combo System**: Cadenas de ataques con diferentes propiedades

#### Ataques Principales:
1. **Five Hit Combo** - Combo básico con ventana de Guard Counter
2. **Waterfowl Dance** - Ataque signature con 3 fases
3. **Jump Thrust** - Ataque de aproximación aérea
4. **Running Triple Slash** - Combo de acercamiento rápido
5. **Uppercut Slam** - Ataque de área con gran daño
6. **Kick Combos** - Ataques de patada con seguimientos

### 🎨 Elementos Visuales

#### Efectos Implementados:
- **Sword Glow**: Brillo de espada durante ataques especiales
- **Impact Effects**: Efectos de impacto en el suelo
- **Camera Shake**: Sacudida de cámara en ataques pesados
- **Damage Indicators**: Indicadores visuales de daño

#### Animaciones:
- **Blend Trees**: Para movimiento fluido
- **State Machine**: Gestión compleja de estados de animación
- **Transition Conditions**: Condiciones precisas entre animaciones

---

## 🏗️ Arquitectura Técnica

### 📁 Estructura del Proyecto

```
Assets/
├── Animators/
│   └── MaleniaAnimatorController.controller
├── Scripts/
│   ├── MaleniaAttacks.cs
│   ├── DamageDealer.cs
│   ├── BossLifeBarScript.cs
│   └── GameManagerScript.cs
├── Models/
│   ├── Malenia/
│   └── LetMeSoloHer/
├── Effects/
│   ├── ImpactPrefab
│   └── SwordGlow
└── Screenshot/
    ├── main.png
    ├── 2.png
    └── 3.png
```

### 🔧 Componentes Clave

#### MaleniaAttacks.cs
El script principal que maneja toda la lógica de combate:

```csharp
public class MaleniaAttacks : MonoBehaviour, INextMove
{
    [Header("AI Manager")]
    public float nearValue;
    public float farValue;
    public float chillTime;
    
    [Header("Waterfowl Dance")]
    private bool waterfowlUsed;
    private float waterfowlHPThreshold = 0.75f;
    
    [Header("Lifesteal System")]
    public void ApplyLifesteal(float healRatio)
    {
        float healAmount = bossLifeBar.maxLife * healRatio;
        bossLifeBar.UpdateLife(healAmount);
    }
}
```

#### Animator Controller
Maneja las transiciones entre estados de animación con parámetros como:
- `Vertical/Horizontal`: Movimiento direccional
- `Attacking`: Estado de ataque activo
- `TakingDamage`: Estado de recibir daño
- `Dead`: Estado de muerte
- Triggers específicos para cada ataque

![Screenshot Sistema](Assets/Screenshot/3.png)

### 🎯 Sistema de Waterfowl Dance

La implementación del ataque más icónico de Malenia:

#### Condiciones de Activación:
- HP menor al 75%
- Distancia entre 8-15 metros
- Cooldown disponible
- No interrumpida

#### Fases del Ataque:
1. **Wind-up** (20 frames): Preparación vulnerable
2. **Flurry 1**: Ráfaga inicial con homing
3. **Flurry 2**: Recálculo y segunda ráfaga
4. **Flurry 3**: Ráfaga final con inversión de eje

```csharp
private bool CanUseWaterfowlDance()
{
    if (waterfowlUsed) return false;
    
    float currentHPRatio = bossLifeBar.GetBossLifeAmount() / bossLifeBar.maxLife;
    bool hpCondition = currentHPRatio <= waterfowlHPThreshold;
    bool distanceCondition = distance >= waterfowlMinDistance && distance <= waterfowlMaxDistance;
    
    return hpCondition && distanceCondition;
}
```

---

## 🎮 Controles y Debugging

### 🕹️ Controles de Debug (Solo Master)

- **Keypad 0**: Toggle AI On/Off
- **Keypad 1**: Activar modo debug
- **Alpha 1-9**: Ataques manuales específicos

### 📊 UI de Debug

El sistema incluye múltiples indicadores visuales:
- **Brain Icon**: Indica si la IA está activa
- **Distance Debug**: Distancia actual al jugador (con colores)
- **Attack State**: Estado actual de ataque
- **Speed Value**: Velocidad de movimiento actual
- **Damage Value**: Cantidad de daño configurada

---

## 🚀 Instalación y Configuración

### ⚙️ Requisitos

- **Unity 2022.3 LTS** o superior
- **Universal Render Pipeline (URP)**
- **Input System Package**

### 📥 Configuración

1. **Clonar el repositorio:**
```bash
git clone https://github.com/yeagob/Malenia_Masterclass.git
cd Malenia_Masterclass
git checkout develop
```

2. **Abrir en Unity:**
   - Abrir Unity Hub
   - Seleccionar "Open Project"
   - Navegar a la carpeta del proyecto

3. **Configurar la escena:**
   - La escena principal está en `Assets/Scenes/`
   - Asegurar que URP está configurado
   - Verificar que todos los prefabs están asignados

### 🎯 Parámetros Configurables

#### En MaleniaAttacks.cs:
- `nearValue`: Distancia para ataques cuerpo a cuerpo
- `farValue`: Distancia para cambiar a modo persecución
- `chillTime`: Tiempo de espera entre acciones
- `waterfowlHPThreshold`: Umbral de HP para Waterfowl Dance

---

## 📈 Frame Data y Balanceo

### ⏱️ Datos Técnicos (60 FPS)

| Ataque | Startup | Active | Recovery | Ventana de Castigo |
|--------|---------|--------|----------|-------------------|
| Five Hit Combo (Hit 3) | 8F | 3F | 18F | **20F** |
| Jump Thrust | 45F | 4F | 35F | 18F |
| Waterfowl Wind-up | 20F | 0F | N/A | N/A |
| Spinning Kick | 25F | 6F | 40F | 25F |

### 🎯 Sistema de Lifesteal

- **Ratio base**: 1.6% del HP máximo por golpe
- **Activación**: En cualquier hit exitoso (incluso bloqueado)
- **Propósito**: Penalizar estrategias defensivas pasivas

---

## 🏆 Créditos y Reconocimientos

### 👨‍💻 Desarrollo Principal
- **Proyecto base**: [Bruno Gottilieb](https://github.com/BrunoGottlieb/Souls-Combat)
- **Adaptación y Masterclass**: [yeagob](https://github.com/yeagob)

### 🎨 Assets y Modelos
- **Malenia Model**: CrimsonGCat
- **Let Me Solo Her Model**: CrimsonGCat
- **Water Shader**: Proto Series (Unity Asset Store)
- **Nature Pack**: Unity Asset Store

### 🎓 Colaboración Educativa
- **[Escuela Musk](https://escuelamusk.com/videojuegos/)**: Patrocinio y apoyo educativo
- **FromSoftware**: Creadores originales de Malenia y Elden Ring

---

## 📝 Licencia y Uso

Este proyecto tiene fines **educativos y de investigación** únicamente. 

- ✅ Uso para aprendizaje y enseñanza
- ✅ Modificación y experimentación
- ✅ Referencia para otros proyectos educativos
- ❌ Uso comercial sin autorización
- ❌ Distribución de assets propietarios

---

## 🤝 Contribuciones

¡Las contribuciones son bienvenidas! Si tienes ideas para mejorar:

1. Fork del proyecto
2. Crear una branch para tu feature
3. Commit de tus cambios
4. Push a la branch
5. Abrir un Pull Request

### 📋 Áreas de Mejora

- [ ] Implementación de Phase 2
- [ ] Mejora de efectos visuales
- [ ] Optimización de performance
- [ ] Sistema de sonido
- [ ] Más variaciones de ataques

---

## 📞 Contacto y Soporte

- **Issues**: [GitHub Issues](https://github.com/yeagob/Malenia_Masterclass/issues)
- **Escuela Musk**: [escuelamusk.com/videojuegos](https://escuelamusk.com/videojuegos/)
- **Documentación adicional**: Ver links en la sección de recursos

---

*Proyecto desarrollado con 💜 para la comunidad de desarrollo de videojuegos*