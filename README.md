Malenia Masterclass - Recreación del Combate Fase 1
Proyecto patrocinado por:
Mostrar imagen
Escuela especializada en desarrollo de videojuegos - escuelamusk.com/videojuegos

📚 Recursos de la Masterclass
Este repositorio fue creado para una Masterclass en directo sobre cómo reproducir el combate contra Malenia de Elden Ring en Unity. Aquí están todos los recursos mencionados durante la sesión:
🔗 Enlaces y Recursos Utilizados
Proyecto Base:

Proyecto original de Bruno Gottilieb - Souls Combat

Modelos 3D:

Modelo Let Me Solo Her (PS1 Style)
Modelo Malenia (PS1 Style)

Assets de Unity:

Shader de Agua Estilizada (URP)
Pack de Plantas y Flores Gratis

Herramientas de Desarrollo:

Generador de Skyboxes con IA
Upscaler de imágenes con IA (Local)
Editor de modelos 3D online
ActorCore - Rigging automático gratuito
Mixamo - Animaciones y poses

Documentación Técnica:

Podcast: Sistema de combate de Malenia
Wiki oficial - Ataques de Malenia
Guía definitiva de ataques
Análisis técnico completo del diseño de Malenia


🎮 Descripción del Proyecto
Mostrar imagen
Esta es una recreación técnica y educativa del icónico combate contra Malenia, Blade of Miquella (Fase 1) de Elden Ring, implementada en Unity. El proyecto fue desarrollado específicamente para demostrar técnicas avanzadas de programación de IA para boss fights y mecánicas de combate complejas.
🎯 Objetivos del Proyecto

Educativo: Enseñar el desarrollo de IA compleja para boss fights
Técnico: Implementar sistemas de combate avanzados
Recreativo: Fidelidad al combate original de Elden Ring
Modular: Código reutilizable para otros proyectos


✨ Características Principales
🤖 Sistema de IA Avanzado
La IA de Malenia utiliza una Máquina de Estados Finitos (FSM) con los siguientes componentes:
Estados Principales:

Idle/Stalk: Movimiento neutro y evaluación de distancia
Melee_Aggressive: Combos cuerpo a cuerpo agresivos
Ranged_Approach: Ataques de acercamiento a media/larga distancia
WFD_Ready: Estado especial para Waterfowl Dance

Lógica de Decisión:
csharp// Ejemplo del sistema de decisión basado en distancia
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
🔧 Componentes Clave
MaleniaAttacks.cs
El script principal que maneja toda la lógica de combate:
csharppublic class MaleniaAttacks : MonoBehaviour, INextMove
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
Animator Controller
Maneja las transiciones entre estados de animación con parámetros como:

Vertical/Horizontal: Movimiento direccional
Attacking: Estado de ataque activo
TakingDamage: Estado de recibir daño
Dead: Estado de muerte
Triggers específicos para cada ataque

Mostrar imagen
🎯 Sistema de Waterfowl Dance
La implementación del ataque más icónico de Malenia:
Condiciones de Activación:

HP menor al 75%
Distancia entre 8-15 metros
Cooldown disponible
No interrumpida

Fases del Ataque:

Wind-up (20 frames): Preparación vulnerable
Flurry 1: Ráfaga inicial con homing
Flurry 2: Recálculo y segunda ráfaga
Flurry 3: Ráfaga final con inversión de eje

csharpprivate bool CanUseWaterfowlDance()
{
    if (waterfowlUsed) return false;
    
    float currentHPRatio = bossLifeBar.GetBossLifeAmount() / bossLifeBar.maxLife;
    bool hpCondition = currentHPRatio <= waterfowlHPThreshold;
    bool distanceCondition = distance >= waterfowlMinDistance && distance <= waterfowlMaxDistance;
    
    return hpCondition && distanceCondition;
}

🎮 Controles y Debugging
🕹️ Controles de Debug (Solo Master)

Keypad 0: Toggle AI On/Off
Keypad 1: Activar modo debug
Alpha 1-9: Ataques manuales específicos

📊 UI de Debug
El sistema incluye múltiples indicadores visuales:

Brain Icon: Indica si la IA está activa
Distance Debug: Distancia actual al jugador (con colores)
Attack State: Estado actual de ataque
Speed Value: Velocidad de movimiento actual
Damage Value: Cantidad de daño configurada


🚀 Instalación y Configuración
⚙️ Requisitos

Unity 2022.3 LTS o superior
Universal Render Pipeline (URP)
Input System Package

📥 Configuración

Clonar el repositorio:

bashgit clone https://github.com/yeagob/Malenia_Masterclass.git
cd Malenia_Masterclass
git checkout develop

Abrir en Unity:

Abrir Unity Hub
Seleccionar "Open Project"
Navegar a la carpeta del proyecto


Configurar la escena:

La escena principal está en Assets/Scenes/
Asegurar que URP está configurado
Verificar que todos los prefabs están asignados



🎯 Parámetros Configurables
En MaleniaAttacks.cs:

nearValue: Distancia para ataques cuerpo a cuerpo
farValue: Distancia para cambiar a modo persecución
chillTime: Tiempo de espera entre acciones
waterfowlHPThreshold: Umbral de HP para Waterfowl Dance


📈 Frame Data y Balanceo
⏱️ Datos Técnicos (60 FPS)
AtaqueStartupActiveRecoveryVentana de CastigoFive Hit Combo (Hit 3)8F3F18F20FJump Thrust45F4F35F18FWaterfowl Wind-up20F0FN/AN/ASpinning Kick25F6F40F25F
🎯 Sistema de Lifesteal

Ratio base: 1.6% del HP máximo por golpe
Activación: En cualquier hit exitoso (incluso bloqueado)
Propósito: Penalizar estrategias defensivas pasivas


🏆 Créditos y Reconocimientos
👨‍💻 Desarrollo Principal

Proyecto base: Bruno Gottilieb
Adaptación y Masterclass: yeagob

🎨 Assets y Modelos

Malenia Model: CrimsonGCat
Let Me Solo Her Model: CrimsonGCat
Water Shader: Proto Series (Unity Asset Store)
Nature Pack: Unity Asset Store

🎓 Colaboración Educativa

Escuela Musk: Patrocinio y apoyo educativo
FromSoftware: Creadores originales de Malenia y Elden Ring


📝 Licencia y Uso
Este proyecto tiene fines educativos y de investigación únicamente.

✅ Uso para aprendizaje y enseñanza
✅ Modificación y experimentación
✅ Referencia para otros proyectos educativos
❌ Uso comercial sin autorización
❌ Distribución de assets propietarios
