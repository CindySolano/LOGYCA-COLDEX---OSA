# MUEVE EL DATO, MUEVE LA GÓNDOLA
## Documentación técnica para desarrollo en Unity

**Proyecto:** Atracción interactiva OSA Colaborativo - LOGYCA Lab COLDEX
**Versión del documento:** 1.0
**Plataforma destino:** Unity (pantalla touch)
**Audiencia:** Desarrollador Unity / equipo de diseño UX

---

## Tabla de contenidos

1. Resumen ejecutivo
2. Concepto y objetivo pedagógico
3. Arquitectura del sistema
4. Flujo de la experiencia
5. Estructura de datos
6. Indicadores y reglas de visualización
7. Especificación de pantallas
8. Guía visual y diseño
9. Implementación en Unity (código)
10. Requisitos técnicos del hardware
11. Pruebas y casos de validación
12. Anexos

---

## 1. Resumen ejecutivo

### 1.1 Qué es

Una experiencia interactiva touch ubicada dentro del LOGYCA Lab que permite al visitante (estudiantes, MiPymes, ejecutivos, público general) tomar decisiones de un retailer frente a problemas reales de disponibilidad en góndola (OSA), y ver en tiempo real cómo cada decisión impacta la operación, las finanzas, al cliente y la relación con el proveedor.

### 1.2 Para qué

Posicionar el concepto de **OSA Colaborativo** como la mejor estrategia para resolver problemas de disponibilidad en góndola. El usuario debe descubrir, a través de la propia interacción, que las decisiones que más impactan positivamente todos los indicadores son aquellas donde la cadena y el proveedor trabajan en equipo.

### 1.3 Cómo funciona

El usuario ve un supermercado con varios "hot points" interactivos. Toca uno, lee la situación que ahí se presenta, elige una de tres decisiones posibles, y recibe feedback completo: indicadores numéricos, impacto en cada actor de la cadena y un veredicto pedagógico.

### 1.4 Alcance del piloto

- 5 estaciones operativas en el piloto (Carnes y lácteos, Panadería, Abarrotes, Caja registradora, Bodega)
- 3 opciones de decisión por estación = 15 combinaciones únicas
- 9 indicadores cuantitativos por opción
- 1 indicador cualitativo (satisfacción del shopper)
- 1 escala de colaboración (4 niveles)
- 3 bloques narrativos de feedback por opción

---

## 2. Concepto y objetivo pedagógico

### 2.1 Mensaje central

> El OSA no se compra, se construye con el proveedor.

La atracción busca que cualquier visitante, sin importar su nivel técnico, salga entendiendo que las decisiones unilaterales (cadena resuelve sola) tienen techo, y que las soluciones donde cadena y proveedor comparten datos, decisiones y responsabilidades producen mejores resultados para ambos.

### 2.2 Diferencia entre colaboración y delegación

Concepto crítico que la atracción debe transmitir:

- **Delegación:** una parte deja todo en manos de la otra. Funciona, pero es transaccional.
- **Colaboración:** ambas partes comparten datos, deciden juntas, comparten riesgos y beneficios. Trabajan como un solo equipo.

Esta distinción se manifiesta principalmente en la estación de Caja registradora, donde la opción B (la marca gestiona toda la zona) es delegación nivel 2, y la opción C (comité conjunto cadena-proveedor) es colaboración nivel 4. El usuario debe sentir el contraste.

### 2.3 Audiencias objetivo

| Tipo de visitante | Necesidad de la atracción |
|---|---|
| Estudiante de colegio | Entender conceptos básicos de comercio y cadena de abastecimiento |
| Estudiante universitario | Vincular teoría con casos reales |
| MiPyme / dueño de tienda | Identificar oportunidades en su propia operación |
| Ejecutivo de empresa grande | Validar conceptos de OSA colaborativo y formar criterio decisorio |
| Público general | Entender el "detrás de cámaras" del supermercado |

**Implicación de diseño:** todo el lenguaje de la atracción debe ser entendible para una persona sin formación técnica. Sin siglas, sin tecnicismos. Las palabras "VMI", "CPFR", "SKU", "lead time", "phantom stockout" no aparecen en pantalla.

---

## 3. Arquitectura del sistema

### 3.1 Componentes principales

```
┌─────────────────────────────────────────────────┐
│              CAPA DE PRESENTACIÓN               │
│  (Unity UI - Canvas, ventanas, animaciones)     │
├─────────────────────────────────────────────────┤
│              CAPA DE LÓGICA                     │
│  (GameManager, StateManager, ScreenController)  │
├─────────────────────────────────────────────────┤
│              CAPA DE DATOS                      │
│  (ScriptableObjects con estaciones y opciones)  │
└─────────────────────────────────────────────────┘
```

### 3.2 Estados de la aplicación

La experiencia funciona como una máquina de estados con 6 estados principales:

| Estado | Descripción | Pantalla visible |
|---|---|---|
| `Attract` | Pantalla de espera con animación llamativa | Atracción inicial |
| `Intro` | Bienvenida y explicación breve | Texto introductorio |
| `MapSelection` | Mapa del supermercado con hot points | Vista del supermercado |
| `Situation` | Contexto y pregunta de la estación | Pantalla de situación |
| `Decision` | Tres opciones para elegir | Botones de decisión |
| `Feedback` | Resultados e implicaciones | Pantalla de resultados |
| `Summary` (opcional) | Resumen al finalizar las 5 estaciones | Cierre |

### 3.3 Flujo de transiciones

```
Attract → [touch] → Intro → MapSelection
   ↑                            ↓
   ↑                       [tap hot point]
   ↑                            ↓
   ↑                        Situation
   ↑                            ↓
   ↑                         Decision
   ↑                            ↓
   ↑                         Feedback
   ↑                            ↓
   ↑                  [back to MapSelection]
   ↑                            ↓
   ←——————— [timeout / 5 visitadas] ←
```

### 3.4 Reset automático

La pantalla debe volver al estado `Attract` después de:
- 60 segundos de inactividad sin tocar la pantalla
- Al completar las 5 estaciones (mostrar resumen → reset a los 30 segundos)

---

## 4. Flujo de la experiencia

### 4.1 Recorrido típico del usuario

**Paso 1 - Atracción (Attract):**
La pantalla muestra una animación llamativa del supermercado con texto "Toca para empezar". Al tocar la pantalla en cualquier lugar, pasa a Intro.

**Paso 2 - Bienvenida (Intro):**
Texto breve: *"Eres el gerente de un supermercado. En cada sección encontrarás un problema real. Tu misión: tomar la mejor decisión. Pero ojo: cada decisión tiene consecuencias."*. Botón único: "Comenzar".

**Paso 3 - Mapa del supermercado (MapSelection):**
Vista isométrica o cenital de un supermercado con 5 zonas marcadas con hot points pulsantes. Texto superior: "Toca una sección para empezar".

Hot points iniciales:
- Carnes y lácteos
- Panadería
- Abarrotes
- Caja registradora
- Bodega

Las estaciones ya visitadas aparecen con un check verde, pero pueden volver a tocarse.

**Paso 4 - Situación (Situation):**
Pantalla con 3 elementos:
- Título grande: nombre de la estación + icono
- Caja gris con el contexto narrativo (entre 2 y 4 líneas)
- Botón único en la parte inferior: "¿Qué decides hacer?"

**Paso 5 - Decisión (Decision):**
Pregunta superior: "¿Qué decides hacer?"
Tres botones grandes apilados verticalmente con las opciones A, B, C. Cada botón muestra:
- Letra (A, B o C) en círculo a la izquierda
- Texto de la opción

Botón inferior: "Volver al mapa" (por si se arrepiente).

**Paso 6 - Feedback (Feedback):**
La pantalla más rica en información. Está dividida en 5 bloques verticales:

1. **Impacto operativo** - 6 KPIs en grid (OSA, días inventario, espacio, tiempo entrega, costo transporte, costo operativo)
2. **Impacto en el bolsillo** - 2 KPIs (ventas, margen)
3. **Impacto en el cliente** - 1 indicador grande (cara feliz/neutra/triste + texto)
4. **Tipo de relación con el proveedor** - barra de 4 segmentos + nombre del nivel
5. **Lo que pasó** - dos cajas lado a lado (cadena | proveedor) + cuadro destacado con el veredicto

Botón inferior: "Volver al mapa" o "Probar otra decisión" (regresa a Decision con la misma estación).

### 4.2 Tiempos esperados

| Estado | Tiempo estimado del usuario |
|---|---|
| Intro | 5-10 segundos |
| MapSelection | 3-8 segundos por elección |
| Situation | 10-15 segundos (lectura) |
| Decision | 10-25 segundos (deliberación) |
| Feedback | 30-60 segundos (lectura completa) |

**Total estimado por estación:** 60-110 segundos
**Total experiencia completa:** 5 a 9 minutos

---

## 5. Estructura de datos

### 5.1 Modelo de datos jerárquico

```
ExperienciaCompleta
└── Estaciones (5)
    ├── Estacion
    │   ├── id: string
    │   ├── nombre: string
    │   ├── subtitulo: string
    │   ├── icono: ref a Sprite
    │   ├── colorPrincipal: Color
    │   ├── posicionEnMapa: Vector2
    │   ├── contexto: string (largo)
    │   ├── pregunta: string
    │   └── Opciones (3)
    │       ├── OpcionDecision
    │       │   ├── letra: char ('A', 'B' o 'C')
    │       │   ├── titulo: string
    │       │   ├── kpis: Lista de KPI
    │       │   ├── nivelColaboracion: int (1-4)
    │       │   ├── feedbackCadena: string
    │       │   ├── feedbackProveedor: string
    │       │   └── veredicto: string
    │       └── ...
    └── ...
```

### 5.2 Catálogo de KPIs

Cada KPI tiene tres atributos: **nombre**, **dirección** y **valor descriptivo**.

| Nombre | Direcciones posibles | Categoría |
|---|---|---|
| Disponibilidad en góndola | Sube / Baja / Igual | Operativo |
| Días de inventario | Baja / Sube / Igual / No aplica | Operativo |
| Espacio en góndola | Sube / Baja / Igual | Operativo |
| Tiempo de entrega | Baja / Sube / Igual / No aplica | Operativo |
| Costo de transporte | Sube / Baja / Igual | Operativo |
| Costo operativo | Sube / Baja / Igual | Operativo |
| Ventas | Sube / Baja / Igual | Bolsillo |
| Margen de ganancia | Sube / Baja / Igual | Bolsillo |
| Satisfacción del shopper | Sube / Baja / Igual | Cliente |

### 5.3 Niveles de colaboración

| Nivel | Nombre | Descripción corta |
|---|---|---|
| 1 | Cada quien por su lado | Decides solo, el proveedor solo recibe el pedido |
| 2 | Coordinación básica | Negocian un cambio puntual, pero cada uno sigue por su cuenta |
| 3 | Datos compartidos | Le abres tus números al proveedor para que decida mejor |
| 4 | Gestión conjunta | Comparten datos, decisiones, riesgos y beneficios. Trabajan como un solo equipo |

### 5.4 Reglas de coloreo de KPIs

La dirección de un KPI no es lo mismo que su impacto positivo o negativo. Por ejemplo:
- "Costo de transporte sube" = malo (verde no, rojo sí)
- "Días de inventario baja" = bueno (verde)
- "Disponibilidad sube" = bueno (verde)

Por eso cada KPI debe definirse con dos atributos:
- **dirección visual:** flecha hacia arriba o hacia abajo
- **interpretación:** favorable (verde) o desfavorable (rojo)

Tabla de mapeo:

| KPI | "Sube" es | "Baja" es |
|---|---|---|
| Disponibilidad en góndola | favorable | desfavorable |
| Días de inventario | desfavorable | favorable |
| Espacio en góndola | favorable | desfavorable |
| Tiempo de entrega | desfavorable | favorable |
| Costo de transporte | desfavorable | favorable |
| Costo operativo | desfavorable | favorable |
| Ventas | favorable | desfavorable |
| Margen de ganancia | favorable | desfavorable |
| Satisfacción del shopper | favorable | desfavorable |

---

## 6. Indicadores y reglas de visualización

### 6.1 Iconografía de KPIs

Cada KPI debe acompañarse de un ícono según su dirección e interpretación:

| Dirección + Interpretación | Ícono | Color |
|---|---|---|
| Sube + Favorable | Flecha diagonal arriba a la derecha | Verde (#3B6D11) |
| Baja + Favorable | Flecha diagonal abajo a la derecha | Verde (#3B6D11) |
| Sube + Desfavorable | Flecha diagonal arriba a la derecha | Rojo (#A32D2D) |
| Baja + Desfavorable | Flecha diagonal abajo a la derecha | Rojo (#A32D2D) |
| Igual / No aplica | Línea horizontal (raya) | Gris (#888780) |

### 6.2 Visualización de "Satisfacción del shopper"

A diferencia del resto de KPIs, este se muestra con una **carita** grande para ser interpretado de un solo vistazo:

| Estado | Ícono | Fondo del card |
|---|---|---|
| Sube | Carita feliz | Verde claro (#EAF3DE) |
| Igual | Carita neutra | Gris claro (#F1EFE8) |
| Baja | Carita triste | Rojo claro (#FCEBEB) |

Esta caja debe ser visualmente más grande que las demás (al menos el doble de altura), porque es el indicador con mayor potencia comunicativa.

### 6.3 Visualización de "Nivel de colaboración"

**Barra de 4 segmentos horizontales** en la parte superior del card:
- Segmentos activos: color azul fuerte (#185FA5)
- Segmentos inactivos: gris claro (#F1EFE8)
- Espaciado entre segmentos: 4 píxeles
- Altura: 14 píxeles
- Esquinas: redondeadas 3px

Debajo de la barra:
- Nombre del nivel (en negrita)
- Descripción corta (en gris)

A la derecha de la etiqueta superior: badge con texto "X de 4" sobre fondo azul claro.

---

## 7. Especificación de pantallas

### 7.1 Pantalla "Attract"

```
┌────────────────────────────────────────┐
│                                        │
│         [Animación supermercado]       │
│                                        │
│        MUEVE EL DATO,                  │
│        MUEVE LA GÓNDOLA                │
│                                        │
│        Toca para empezar               │
│                                        │
└────────────────────────────────────────┘
```

- Animación de fondo: subtle, tipo loop de 8-10 segundos.
- Tipografía del título: grande (96-120pt).
- "Toca para empezar" debe pulsar suavemente (opacidad de 60% a 100% cada 2s).

### 7.2 Pantalla "MapSelection"

```
┌──────────────────────────────────────────────┐
│  LOGYCA LAB · OSA Colaborativo               │
│  Toca una sección para empezar               │
├──────────────────────────────────────────────┤
│                                              │
│   [Mapa isométrico del supermercado]         │
│                                              │
│   ●Carnes  ●Panadería  ●Abarrotes            │
│            ●Caja      ●Bodega                │
│                                              │
└──────────────────────────────────────────────┘
```

- Hot points: círculos pulsantes con icono de la categoría adentro.
- Estaciones visitadas: muestran un check verde sobre el hot point.
- Color de cada hot point: según el color principal definido para cada estación.

### 7.3 Pantalla "Situation"

```
┌──────────────────────────────────────────────┐
│  ← Mapa                                      │
├──────────────────────────────────────────────┤
│                                              │
│  [Icono]  Carnes y lácteos                   │
│           Cadena de frío                     │
│                                              │
│  ┌────────────────────────────────────────┐  │
│  │ SITUACIÓN                              │  │
│  │                                        │  │
│  │ Esta semana se dañó el 9% de las       │  │
│  │ carnes empacadas porque se vencieron   │  │
│  │ antes de venderse...                   │  │
│  └────────────────────────────────────────┘  │
│                                              │
│         [¿Qué decides hacer? →]              │
│                                              │
└──────────────────────────────────────────────┘
```

### 7.4 Pantalla "Decision"

```
┌──────────────────────────────────────────────┐
│  ← Volver                                    │
├──────────────────────────────────────────────┤
│                                              │
│  ¿Qué decides hacer?                         │
│                                              │
│  ┌────────────────────────────────────────┐  │
│  │ [A]  Pedirle al proveedor que          │  │
│  │      entregue 3 veces por semana       │  │
│  └────────────────────────────────────────┘  │
│                                              │
│  ┌────────────────────────────────────────┐  │
│  │ [B]  Trabajar en equipo con el         │  │
│  │      proveedor: comparten datos...     │  │
│  └────────────────────────────────────────┘  │
│                                              │
│  ┌────────────────────────────────────────┐  │
│  │ [C]  Seguir igual pero rebajar las     │  │
│  │      carnes que están por vencerse     │  │
│  └────────────────────────────────────────┘  │
│                                              │
└──────────────────────────────────────────────┘
```

- Cada botón mide al menos 90px de altura para ser fácilmente tocable.
- El círculo con la letra debe ser claramente visible (40px diámetro, color de marca).
- Hover/touch feedback: el botón cambia de fondo blanco a fondo azul claro al ser tocado.

### 7.5 Pantalla "Feedback"

Esta es la pantalla más densa en información. Debe usar scroll vertical si es necesario, pero idealmente cabe en una pantalla 1080p horizontal.

```
┌──────────────────────────────────────────────────────────┐
│  ← Volver al mapa                  [Probar otra decisión]│
├──────────────────────────────────────────────────────────┤
│                                                          │
│  Elegiste: A) Pedirle al proveedor que entregue 3x       │
│                                                          │
│  ⚙ IMPACTO OPERATIVO                                    │
│  ┌──────────┬──────────┬──────────┐                      │
│  │ ↗ OSA    │ ↘ Días   │ → Espacio│                      │
│  │ 91→95%   │ 7→5      │ Igual    │                      │
│  ├──────────┼──────────┼──────────┤                      │
│  │ ↘ Tiempo │ ↗ Transp │ → Opex   │                      │
│  │ 4→3 días │ +30%     │ Igual    │                      │
│  └──────────┴──────────┴──────────┘                      │
│                                                          │
│  💰 IMPACTO EN EL BOLSILLO                              │
│  ┌──────────────────┬──────────────────┐                 │
│  │ ↗ Ventas: +5%    │ → Margen: igual  │                 │
│  └──────────────────┴──────────────────┘                 │
│                                                          │
│  ❤ IMPACTO EN EL CLIENTE                                │
│  ┌────────────────────────────────────┐                  │
│  │   😊  Satisfacción: SUBE           │                  │
│  └────────────────────────────────────┘                  │
│                                                          │
│  🤝 TIPO DE RELACIÓN CON EL PROVEEDOR                   │
│  ┌────────────────────────────────────┐                  │
│  │ ▓▓░░  2 de 4                       │                  │
│  │ Coordinación básica                 │                  │
│  └────────────────────────────────────┘                  │
│                                                          │
│  💬 LO QUE PASÓ                                         │
│  ┌────────────────────┬──────────────────┐               │
│  │ 🏪 PARA LA CADENA  │ 🚚 PARA EL PROV. │               │
│  │ Vendiste un poco...│ Está poniendo... │               │
│  └────────────────────┴──────────────────┘               │
│                                                          │
│  ┌────────────────────────────────────────┐              │
│  │ 💡 EL VEREDICTO                       │              │
│  │ La cadena gana poquito, el proveedor  │              │
│  │ pierde. Esto no es sostenible...      │              │
│  └────────────────────────────────────────┘              │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

### 7.6 Pantalla "Summary" (opcional)

Si el usuario completó las 5 estaciones, mostrar:

```
┌──────────────────────────────────────────────┐
│                                              │
│       ¡Recorriste el supermercado!           │
│                                              │
│  Tu perfil de decisor:                       │
│  ┌────────────────────────────────────┐      │
│  │ COLABORADOR                         │      │
│  │ Promedio nivel de colaboración: 3.2 │      │
│  └────────────────────────────────────┘      │
│                                              │
│  Total ganado en margen: +12%                │
│  Satisfacción del cliente: SUBE              │
│                                              │
│  [ Empezar de nuevo ]                        │
│                                              │
└──────────────────────────────────────────────┘
```

Categorías de perfil según promedio de colaboración:
- 1.0 - 1.5: "Lobo solitario"
- 1.6 - 2.5: "Operador clásico"
- 2.6 - 3.4: "Colaborador"
- 3.5 - 4.0: "Jugador en equipo"

---

## 8. Guía visual y diseño

### 8.1 Paleta de colores

**Colores principales (LOGYCA y marca):**

| Uso | Color | Hex |
|---|---|---|
| Color marca LOGYCA (acento) | Naranja LOGYCA | #FF895C |
| Color marca COLDEX | Azul oscuro | #042C53 |
| Background general | Blanco hueso | #FAFAF7 |
| Background secundario (cards) | Gris muy claro | #F1EFE8 |
| Texto principal | Negro suave | #2C2C2A |
| Texto secundario | Gris medio | #5F5E5A |
| Texto terciario / hint | Gris claro | #888780 |

**Colores semánticos (positivo/negativo):**

| Uso | Color | Hex |
|---|---|---|
| Favorable (texto verde) | Verde oscuro | #3B6D11 |
| Favorable (fondo verde) | Verde claro | #EAF3DE |
| Desfavorable (texto rojo) | Rojo oscuro | #A32D2D |
| Desfavorable (fondo rojo) | Rojo claro | #FCEBEB |
| Neutral / igual | Gris medio | #888780 |

**Color por estación (para hot points y headers):**

| Estación | Color | Hex |
|---|---|---|
| Carnes y lácteos | Rojo carmín | #C44848 |
| Panadería | Ámbar / mostaza | #BA7517 |
| Abarrotes | Verde azulado | #1D9E75 |
| Caja registradora | Púrpura | #7F77DD |
| Bodega | Azul medio | #378ADD |

**Color informativo (para boxes destacados):**

| Uso | Color | Hex |
|---|---|---|
| Background info (boxes destacados) | Azul muy claro | #E6F1FB |
| Texto info | Azul oscuro | #0C447C |

### 8.2 Tipografía

**Familia recomendada:** Sans-serif geométrica y legible. Opciones:
- Inter (gratuita, Google Fonts)
- Montserrat (gratuita, Google Fonts)
- Open Sans (gratuita, Google Fonts)

**Jerarquía:**

| Elemento | Tamaño (en pantalla 1080p) | Peso |
|---|---|---|
| Título principal Attract | 96-120pt | Regular |
| Título de pantalla (H1) | 48pt | Bold |
| Subtítulo de pantalla (H2) | 32pt | Regular |
| Encabezado de sección | 24pt | Bold |
| Cuerpo de texto | 22-24pt | Regular |
| Texto secundario | 18-20pt | Regular |
| Etiquetas / labels | 16pt | Bold (mayúsculas) |
| Texto de KPI valor | 18-20pt | Bold |
| Texto de KPI nombre | 14-16pt | Regular |

**Importante:** todos los tamaños deben ser proporcionalmente más grandes que en una interfaz de escritorio normal, porque la pantalla es táctil y la gente la mira a una distancia cómoda (60-80cm).

### 8.3 Espaciado y layout

- Márgenes laterales mínimos en pantalla: 64px
- Espaciado entre cards / bloques: 24px
- Padding interno de cards: 16-24px
- Border-radius estándar: 12px (cards), 20px (botones grandes), 50% (badges circulares)
- Bordes: 1px sólido en color #DDDBD3 para divisiones suaves

### 8.4 Botones tactiles

**Botón primario (acción principal):**
- Background: Naranja LOGYCA (#FF895C)
- Texto: blanco, peso bold
- Altura mínima: 80px
- Padding horizontal: 32px
- Border-radius: 20px

**Botón secundario (volver, opciones):**
- Background: blanco
- Borde: 2px solid color de marca
- Texto: color de marca, peso regular
- Mismo dimensionado

**Botón de opción (decision):**
- Background: blanco con borde de 1px gris claro
- En estado seleccionado: background azul claro (#E6F1FB), borde 2px azul (#185FA5)
- Altura mínima: 100px (porque tiene texto largo en wrap)
- Letra (A/B/C) en círculo de 48px diámetro al lado izquierdo

### 8.5 Iconografía

Familia recomendada: **Tabler Icons** (5800+ iconos, gratuita, estilo outline limpio).

Iconos clave de la atracción:

| Concepto | Icono Tabler | Uso |
|---|---|---|
| Carnes | ti-meat | Estación carnes |
| Pan | ti-bread | Estación panadería |
| Botella | ti-bottle | Estación abarrotes |
| Carrito | ti-shopping-cart | Estación caja |
| Caja | ti-package | Estación bodega |
| Engranaje | ti-settings | Bloque operativo |
| Moneda | ti-coin | Bloque bolsillo |
| Corazón | ti-user-heart | Bloque cliente |
| Conexión | ti-affiliate | Bloque colaboración |
| Mensaje | ti-message-circle | Bloque feedback |
| Bombilla | ti-bulb | Veredicto |
| Tienda | ti-building-store | Cadena (en feedback) |
| Camión | ti-truck-delivery | Proveedor (en feedback) |
| Cara feliz | ti-mood-happy | Shopper sube |
| Cara neutra | ti-mood-neutral | Shopper igual |
| Cara triste | ti-mood-sad | Shopper baja |
| Flecha arriba diagonal | ti-arrow-up-right | KPI sube |
| Flecha abajo diagonal | ti-arrow-down-right | KPI baja |
| Línea horizontal | ti-minus | KPI igual |

---

## 9. Implementación en Unity (código)

### 9.1 Estructura de proyecto recomendada

```
Assets/
├── Scripts/
│   ├── Core/
│   │   ├── GameManager.cs
│   │   ├── ScreenController.cs
│   │   └── AppState.cs
│   ├── Data/
│   │   ├── EstacionData.cs (ScriptableObject)
│   │   ├── OpcionData.cs
│   │   ├── KPIData.cs
│   │   └── NivelColaboracion.cs (enum)
│   ├── UI/
│   │   ├── AttractScreen.cs
│   │   ├── MapScreen.cs
│   │   ├── SituationScreen.cs
│   │   ├── DecisionScreen.cs
│   │   ├── FeedbackScreen.cs
│   │   ├── KPICard.cs
│   │   ├── ShopperCard.cs
│   │   └── ColaboracionCard.cs
│   └── Utils/
│       └── IdleTimer.cs
├── ScriptableObjects/
│   └── Estaciones/
│       ├── 01_Carnes.asset
│       ├── 02_Panaderia.asset
│       ├── 03_Abarrotes.asset
│       ├── 04_Caja.asset
│       └── 05_Bodega.asset
├── Sprites/
│   ├── Iconos/
│   ├── Estaciones/
│   └── UI/
├── Fonts/
└── Scenes/
    └── Main.unity
```

### 9.2 ScriptableObjects para datos

**KPIData.cs (estructura serializable, no es un asset):**

```csharp
using System;
using UnityEngine;

[Serializable]
public class KPIData
{
    public TipoKPI tipo;
    public Direccion direccion;
    public string valorDescriptivo;
}

public enum TipoKPI
{
    DisponibilidadGondola,
    DiasInventario,
    EspacioGondola,
    TiempoEntrega,
    CostoTransporte,
    CostoOperativo,
    Ventas,
    MargenGanancia,
    SatisfaccionShopper
}

public enum Direccion
{
    Sube,
    Baja,
    Igual,
    NoAplica
}
```

**OpcionData.cs:**

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OpcionData
{
    [Header("Identificación")]
    public char letra; // 'A', 'B' o 'C'
    public string titulo;

    [Header("Indicadores")]
    public List<KPIData> kpis;

    [Header("Colaboración")]
    [Range(1, 4)]
    public int nivelColaboracion;

    [Header("Feedback narrativo")]
    [TextArea(3, 5)]
    public string feedbackCadena;
    [TextArea(3, 5)]
    public string feedbackProveedor;
    [TextArea(3, 5)]
    public string veredicto;
}
```

**EstacionData.cs (ScriptableObject — sí es asset):**

```csharp
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Estacion", menuName = "LOGYCA/Estación", order = 1)]
public class EstacionData : ScriptableObject
{
    [Header("Identificación")]
    public string id;
    public string nombre;
    public string subtitulo;
    public Sprite icono;
    public Color colorPrincipal;
    public Vector2 posicionEnMapa;

    [Header("Contenido")]
    [TextArea(3, 6)]
    public string contexto;
    public string pregunta = "¿Qué decides hacer?";

    [Header("Opciones de decisión")]
    public List<OpcionData> opciones; // exactamente 3
}
```

### 9.3 Catálogo central de configuración

**ConfiguracionCatalogo.cs:**

```csharp
using UnityEngine;

public static class ConfiguracionCatalogo
{
    // Reglas de interpretación de KPIs (sube = bueno o malo)
    public static bool EsFavorableSubir(TipoKPI tipo)
    {
        switch (tipo)
        {
            case TipoKPI.DisponibilidadGondola:
            case TipoKPI.EspacioGondola:
            case TipoKPI.Ventas:
            case TipoKPI.MargenGanancia:
            case TipoKPI.SatisfaccionShopper:
                return true; // sube = bueno
            case TipoKPI.DiasInventario:
            case TipoKPI.TiempoEntrega:
            case TipoKPI.CostoTransporte:
            case TipoKPI.CostoOperativo:
                return false; // sube = malo
            default:
                return true;
        }
    }

    public static string NombreLegible(TipoKPI tipo)
    {
        switch (tipo)
        {
            case TipoKPI.DisponibilidadGondola: return "Disponibilidad en góndola";
            case TipoKPI.DiasInventario:        return "Días de inventario";
            case TipoKPI.EspacioGondola:        return "Espacio en góndola";
            case TipoKPI.TiempoEntrega:         return "Tiempo de entrega";
            case TipoKPI.CostoTransporte:       return "Costo de transporte";
            case TipoKPI.CostoOperativo:        return "Costo operativo";
            case TipoKPI.Ventas:                return "Ventas";
            case TipoKPI.MargenGanancia:        return "Margen de ganancia";
            case TipoKPI.SatisfaccionShopper:   return "Satisfacción del shopper";
            default: return tipo.ToString();
        }
    }

    public static CategoriaKPI Categoria(TipoKPI tipo)
    {
        if (tipo == TipoKPI.Ventas || tipo == TipoKPI.MargenGanancia)
            return CategoriaKPI.Bolsillo;
        if (tipo == TipoKPI.SatisfaccionShopper)
            return CategoriaKPI.Cliente;
        return CategoriaKPI.Operativo;
    }

    public static (string nombre, string descripcion) NivelColaboracion(int nivel)
    {
        switch (nivel)
        {
            case 1: return ("Cada quien por su lado", "Decides solo, el proveedor solo recibe el pedido");
            case 2: return ("Coordinación básica", "Negocian un cambio puntual, pero cada uno sigue por su cuenta");
            case 3: return ("Datos compartidos", "Le abres tus números al proveedor para que decida mejor");
            case 4: return ("Gestión conjunta", "Comparten datos, decisiones, riesgos y beneficios. Trabajan como un solo equipo");
            default: return ("", "");
        }
    }
}

public enum CategoriaKPI { Operativo, Bolsillo, Cliente }
```

### 9.4 Máquina de estados principal

**AppState.cs:**

```csharp
public enum AppState
{
    Attract,
    Intro,
    MapSelection,
    Situation,
    Decision,
    Feedback,
    Summary
}
```

**GameManager.cs:**

```csharp
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Datos")]
    [SerializeField] private List<EstacionData> estaciones;

    [Header("Pantallas")]
    [SerializeField] private AttractScreen attractScreen;
    [SerializeField] private MapScreen mapScreen;
    [SerializeField] private SituationScreen situationScreen;
    [SerializeField] private DecisionScreen decisionScreen;
    [SerializeField] private FeedbackScreen feedbackScreen;
    [SerializeField] private SummaryScreen summaryScreen;

    [Header("Configuración")]
    [SerializeField] private float idleTimeoutSeconds = 60f;

    public AppState State { get; private set; }
    public EstacionData EstacionActual { get; private set; }
    public OpcionData OpcionActual { get; private set; }

    private HashSet<string> estacionesVisitadas = new HashSet<string>();
    private List<int> nivelesColaboracionElegidos = new List<int>();
    private float lastInteractionTime;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        IrA(AppState.Attract);
    }

    private void Update()
    {
        // Reset por inactividad
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            lastInteractionTime = Time.time;

        if (State != AppState.Attract && Time.time - lastInteractionTime > idleTimeoutSeconds)
        {
            ReiniciarExperiencia();
        }
    }

    public void IrA(AppState nuevoEstado)
    {
        State = nuevoEstado;
        OcultarTodasLasPantallas();
        switch (nuevoEstado)
        {
            case AppState.Attract:     attractScreen.gameObject.SetActive(true); break;
            case AppState.Intro:       /* implementar IntroScreen si se desea */ break;
            case AppState.MapSelection: mapScreen.Mostrar(estacionesVisitadas); break;
            case AppState.Situation:   situationScreen.Mostrar(EstacionActual); break;
            case AppState.Decision:    decisionScreen.Mostrar(EstacionActual); break;
            case AppState.Feedback:    feedbackScreen.Mostrar(EstacionActual, OpcionActual); break;
            case AppState.Summary:     summaryScreen.Mostrar(nivelesColaboracionElegidos); break;
        }
        lastInteractionTime = Time.time;
    }

    public void SeleccionarEstacion(EstacionData estacion)
    {
        EstacionActual = estacion;
        IrA(AppState.Situation);
    }

    public void SeleccionarOpcion(OpcionData opcion)
    {
        OpcionActual = opcion;
        nivelesColaboracionElegidos.Add(opcion.nivelColaboracion);
        estacionesVisitadas.Add(EstacionActual.id);
        IrA(AppState.Feedback);
    }

    public void VolverAlMapa()
    {
        if (estacionesVisitadas.Count >= estaciones.Count)
            IrA(AppState.Summary);
        else
            IrA(AppState.MapSelection);
    }

    public void ReiniciarExperiencia()
    {
        estacionesVisitadas.Clear();
        nivelesColaboracionElegidos.Clear();
        EstacionActual = null;
        OpcionActual = null;
        IrA(AppState.Attract);
    }

    private void OcultarTodasLasPantallas()
    {
        attractScreen.gameObject.SetActive(false);
        mapScreen.gameObject.SetActive(false);
        situationScreen.gameObject.SetActive(false);
        decisionScreen.gameObject.SetActive(false);
        feedbackScreen.gameObject.SetActive(false);
        summaryScreen.gameObject.SetActive(false);
    }
}
```

### 9.5 Componente de KPI Card

**KPICard.cs:**

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KPICard : MonoBehaviour
{
    [SerializeField] private Image iconoFlecha;
    [SerializeField] private TMP_Text textoNombre;
    [SerializeField] private TMP_Text textoValor;

    [Header("Sprites")]
    [SerializeField] private Sprite spriteFlechaArriba;
    [SerializeField] private Sprite spriteFlechaAbajo;
    [SerializeField] private Sprite spriteRaya;

    [Header("Colores")]
    [SerializeField] private Color colorFavorable = new Color(0.23f, 0.43f, 0.07f); // verde
    [SerializeField] private Color colorDesfavorable = new Color(0.64f, 0.18f, 0.18f); // rojo
    [SerializeField] private Color colorNeutral = new Color(0.53f, 0.53f, 0.50f); // gris

    public void Configurar(KPIData kpi)
    {
        textoNombre.text = ConfiguracionCatalogo.NombreLegible(kpi.tipo);
        textoValor.text = kpi.valorDescriptivo;

        bool subirEsFavorable = ConfiguracionCatalogo.EsFavorableSubir(kpi.tipo);
        Color color;
        Sprite icono;

        switch (kpi.direccion)
        {
            case Direccion.Sube:
                icono = spriteFlechaArriba;
                color = subirEsFavorable ? colorFavorable : colorDesfavorable;
                break;
            case Direccion.Baja:
                icono = spriteFlechaAbajo;
                color = subirEsFavorable ? colorDesfavorable : colorFavorable;
                break;
            default:
                icono = spriteRaya;
                color = colorNeutral;
                break;
        }

        iconoFlecha.sprite = icono;
        iconoFlecha.color = color;
    }
}
```

### 9.6 Componente de pantalla Decision

**DecisionScreen.cs:**

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DecisionScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text textoPregunta;
    [SerializeField] private List<OpcionButton> botonesOpcion; // pre-asignados, 3 botones

    public void Mostrar(EstacionData estacion)
    {
        gameObject.SetActive(true);
        textoPregunta.text = estacion.pregunta;

        for (int i = 0; i < botonesOpcion.Count; i++)
        {
            if (i < estacion.opciones.Count)
            {
                botonesOpcion[i].gameObject.SetActive(true);
                botonesOpcion[i].Configurar(estacion.opciones[i],
                    () => GameManager.Instance.SeleccionarOpcion(estacion.opciones[i]));
            }
            else
            {
                botonesOpcion[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnVolverPressed()
    {
        GameManager.Instance.IrA(AppState.MapSelection);
    }
}

public class OpcionButton : MonoBehaviour
{
    [SerializeField] private TMP_Text textoLetra;
    [SerializeField] private TMP_Text textoOpcion;
    [SerializeField] private Button button;

    public void Configurar(OpcionData opcion, System.Action onClick)
    {
        textoLetra.text = opcion.letra.ToString();
        textoOpcion.text = opcion.titulo;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick());
    }
}
```

### 9.7 Componente de pantalla Feedback

**FeedbackScreen.cs:**

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class FeedbackScreen : MonoBehaviour
{
    [Header("Cabecera")]
    [SerializeField] private TMP_Text textoEleccion;

    [Header("KPIs operativos")]
    [SerializeField] private Transform contenedorOperativos;
    [SerializeField] private GameObject prefabKPICard;

    [Header("KPIs bolsillo")]
    [SerializeField] private Transform contenedorBolsillo;

    [Header("KPI cliente")]
    [SerializeField] private ShopperCard shopperCard;

    [Header("Colaboración")]
    [SerializeField] private ColaboracionCard colaboracionCard;

    [Header("Feedback narrativo")]
    [SerializeField] private TMP_Text textoFeedbackCadena;
    [SerializeField] private TMP_Text textoFeedbackProveedor;
    [SerializeField] private TMP_Text textoVeredicto;

    public void Mostrar(EstacionData estacion, OpcionData opcion)
    {
        gameObject.SetActive(true);
        textoEleccion.text = $"Elegiste: {opcion.letra}) {opcion.titulo}";

        LimpiarContenedor(contenedorOperativos);
        LimpiarContenedor(contenedorBolsillo);

        foreach (var kpi in opcion.kpis)
        {
            var categoria = ConfiguracionCatalogo.Categoria(kpi.tipo);

            if (categoria == CategoriaKPI.Operativo)
            {
                var card = Instantiate(prefabKPICard, contenedorOperativos);
                card.GetComponent<KPICard>().Configurar(kpi);
            }
            else if (categoria == CategoriaKPI.Bolsillo)
            {
                var card = Instantiate(prefabKPICard, contenedorBolsillo);
                card.GetComponent<KPICard>().Configurar(kpi);
            }
            else if (categoria == CategoriaKPI.Cliente)
            {
                shopperCard.Configurar(kpi);
            }
        }

        colaboracionCard.Configurar(opcion.nivelColaboracion);
        textoFeedbackCadena.text = opcion.feedbackCadena;
        textoFeedbackProveedor.text = opcion.feedbackProveedor;
        textoVeredicto.text = opcion.veredicto;
    }

    public void OnVolverPressed()
    {
        GameManager.Instance.VolverAlMapa();
    }

    public void OnProbarOtraPressed()
    {
        GameManager.Instance.IrA(AppState.Decision);
    }

    private void LimpiarContenedor(Transform contenedor)
    {
        foreach (Transform child in contenedor) Destroy(child.gameObject);
    }
}
```

### 9.8 Componente de barra de colaboración

**ColaboracionCard.cs:**

```csharp
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ColaboracionCard : MonoBehaviour
{
    [SerializeField] private List<Image> segmentos; // 4 segmentos pre-asignados
    [SerializeField] private TMP_Text textoNivel;
    [SerializeField] private TMP_Text textoNombre;
    [SerializeField] private TMP_Text textoDescripcion;

    [SerializeField] private Color colorActivo = new Color(0.09f, 0.37f, 0.65f);
    [SerializeField] private Color colorInactivo = new Color(0.95f, 0.94f, 0.91f);

    public void Configurar(int nivel)
    {
        textoNivel.text = $"{nivel} de 4";

        var (nombre, desc) = ConfiguracionCatalogo.NivelColaboracion(nivel);
        textoNombre.text = nombre;
        textoDescripcion.text = desc;

        for (int i = 0; i < segmentos.Count; i++)
        {
            segmentos[i].color = (i < nivel) ? colorActivo : colorInactivo;
        }
    }
}
```

### 9.9 Datos completos para los ScriptableObjects

A continuación, los datos exactos para cada estación. Estos se cargan manualmente en los assets `.asset` desde el Inspector de Unity (o pueden importarse mediante un script de editor que parsee el archivo Excel).

#### Estación 1: Carnes y lácteos

**Contexto:** Esta semana se dañó el 9% de las carnes empacadas porque se vencieron antes de venderse. Y al mismo tiempo, hubo 3 días en los que se acabó la leche entera de 1 litro y los clientes no la encontraron. El proveedor entrega 2 veces por semana.

**Opción A - Pedirle al proveedor que entregue 3 veces por semana (Nivel 2)**
- Disponibilidad: Sube — "Sube de 91% a 95%"
- Días de inventario: Baja — "Baja de 7 a 5 días"
- Espacio en góndola: Igual — "Igual"
- Tiempo de entrega: Baja — "Baja de 4 a 3 días"
- Costo de transporte: Sube — "+30%"
- Costo operativo: Igual — "Igual"
- Ventas: Sube — "+5% (menos quiebres)"
- Margen: Igual — "Casi igual (lo que ganas se va en transporte)"
- Shopper: Sube — "Sube"
- Cadena: "Vendiste un poco más por menos quiebres, pero el camión extra se comió casi toda la ganancia. Resolviste hoy, pero a un costo alto."
- Proveedor: "Está poniendo más camiones y planeando más despachos sin recibir nada extra a cambio. La carga operativa subió, su margen también se apretó."
- Veredicto: "La cadena gana poquito, el proveedor pierde. Esto no es sostenible: tarde o temprano él te subirá el precio o pedirá renegociar."

**Opción B - Trabajar en equipo con el proveedor (Nivel 4)**
- Disponibilidad: Sube — "Sube de 91% a 97%"
- Días de inventario: Baja — "Baja de 7 a 5 días"
- Espacio en góndola: Igual — "Igual"
- Tiempo de entrega: Baja — "Baja de 4 a 2 días"
- Costo de transporte: Sube — "+12%"
- Costo operativo: Sube — "+3% (reuniones y software compartido)"
- Ventas: Sube — "+8% (mejor surtido y menos merma)"
- Margen: Sube — "+10% (vendes más y gastas menos)"
- Shopper: Sube — "Sube"
- Cadena: "Vendiste más, gastaste menos en logística, casi no se dañó producto y el cliente siempre encuentra lo que busca. Hay que abrir información y dedicar tiempo a la gestión conjunta, pero el resultado lo justifica."
- Proveedor: "Planea su producción con datos reales y participa en las decisiones de pedido. Despacha lo justo, hace menos viajes en vacío, reduce devoluciones por vencimiento. Sus costos bajan también."
- Veredicto: "Ganan los dos. Esta es la esencia del OSA colaborativo: cuando ambos comparten información Y deciden juntos, el sistema completo se vuelve más eficiente y la torta crece para ambos."

**Opción C - Seguir igual y rebajar las carnes (Nivel 1)**
- Disponibilidad: Baja — "Baja de 91% a 89%"
- Días de inventario: Igual — "Igual, 7 días"
- Espacio en góndola: Baja — "-1 cara"
- Tiempo de entrega: Igual — "Igual"
- Costo de transporte: Igual — "Igual"
- Costo operativo: Igual — "Igual"
- Ventas: Igual — "Casi igual (vendes lo mismo más barato)"
- Margen: Baja — "-6% (descuentos comen la ganancia)"
- Shopper: Igual — "Igual"
- Cadena: "Tapaste el hueco de hoy, pero rebajar precios te bajó la ganancia. Y la próxima semana vuelve a pasar."
- Proveedor: "No se enteró de la merma ni del problema. Va a seguir despachando igual, así que el ciclo de pérdida se repite."
- Veredicto: "Pierden los dos, solo que el proveedor todavía no lo sabe. El problema estructural sigue intacto y va a volver semana tras semana."

#### Estación 2: Panadería

**Contexto:** El pan campesino se hornea 2 veces al día, a las 6 a.m. y a las 2 p.m. Entre 5 y 7 de la tarde casi siempre se acaba y los clientes se van con las manos vacías. Pero al final del día sobra el 15% del pan y toca botarlo.

**Opción A - Tercera horneada a las 5 p.m. (Nivel 1)**
- Disponibilidad: Sube — "Sube de 78% a 86% en la tarde"
- Días de inventario: Igual — "No aplica"
- Espacio en góndola: Sube — "+2 caras en la tarde"
- Tiempo de entrega: Igual — "No aplica"
- Costo de transporte: Igual — "Igual"
- Costo operativo: Sube — "+18% (panadero y energía extra)"
- Ventas: Sube — "+12% (capturas la franja de la tarde)"
- Margen: Sube — "+4% (apenas, panadero y luz cuestan)"
- Shopper: Sube — "Sube"
- Cadena: "Vendes más en la tarde y la gente sale feliz con su pan calientico. Pero el panadero extra y la luz se llevan buena parte de la ganancia."
- Proveedor: "El proveedor de harina vende un poco más, pero sin ningún plan especial. Para él es una venta normal, no hay relación distinta."
- Veredicto: "La cadena gana apenas. El proveedor de harina no nota la diferencia. Es una solución interna que no aprovecha la relación con quien te abastece."

**Opción B - Hornear con datos por hora (Nivel 1)**
- Disponibilidad: Sube — "Sube de 78% a 83%"
- Días de inventario: Igual — "No aplica"
- Espacio en góndola: Igual — "Igual"
- Tiempo de entrega: Igual — "No aplica"
- Costo de transporte: Igual — "Igual"
- Costo operativo: Baja — "-5% (menos desperdicio)"
- Ventas: Sube — "+6%"
- Margen: Sube — "+14% (vendes más y botas menos)"
- Shopper: Sube — "Sube"
- Cadena: "Vendes más, botas mucho menos pan y el cliente encuentra lo que busca, sin contratar a nadie. Es una de las decisiones más rentables."
- Proveedor: "No le pasa nada distinto. Sigue vendiendo harina como siempre."
- Veredicto: "La cadena gana mucho con datos propios, pero la oportunidad colaborativa queda sobre la mesa. Imagínate si compartes ese pronóstico con el molino."

**Opción C - Planear juntos con el molino (Nivel 4)**
- Disponibilidad: Sube — "Sube de 78% a 88%"
- Días de inventario: Baja — "Baja de 5 a 2 días de harina"
- Espacio en góndola: Sube — "+3 caras (más variedad)"
- Tiempo de entrega: Baja — "Baja de 3 a 1 día"
- Costo de transporte: Sube — "+8% (compartido con el proveedor)"
- Costo operativo: Sube — "+4% (tiempo de planeación conjunta)"
- Ventas: Sube — "+18% (capturas tarde y matinal)"
- Margen: Sube — "+22%"
- Shopper: Sube — "Sube"
- Cadena: "Botas casi nada, vendes mucho más, capturas todas las franjas del día y liberas espacio que tenías ocupado con bultos de harina. La inversión es tiempo de gestión conjunta."
- Proveedor: "El molino hace rutas optimizadas con varias panaderías cercanas, planifica producción con datos reales y vende más harina porque tú vendes más pan. Su utilización de transporte sube y reduce devoluciones."
- Veredicto: "Ganan los dos. La harina ya no es solo un commodity: hay un pacto de información Y de decisiones conjuntas que permite que ambos optimicen producción y venta. Pequeño cambio, gran impacto."

#### Estación 3: Abarrotes

**Contexto:** En la sección de aceites tienes 38 referencias distintas. La plata invertida en este inventario subió 18% este año y 6 referencias se venden menos de 2 unidades por semana. El cliente sí encuentra lo que busca, pero estás cargando con mucho inventario que no se mueve.

**Opción A - Sacar las 6 referencias de baja rotación (Nivel 1)**
- Disponibilidad: Sube — "Sube de 96% a 97%"
- Días de inventario: Baja — "Baja de 45 a 32 días"
- Espacio en góndola: Sube — "+30% para las que sí se venden"
- Tiempo de entrega: Igual — "Igual"
- Costo de transporte: Baja — "-4%"
- Costo operativo: Igual — "Igual"
- Ventas: Sube — "+3% (las top tienen más visibilidad)"
- Margen: Sube — "+11% (menos plata congelada)"
- Shopper: Baja — "Baja"
- Cadena: "Liberaste plata y los productos top se ven mejor. Vendes un poco más. Pero el cliente fiel que buscaba esa marca específica se va molesto."
- Proveedor: "Las marcas que sacaste pierden su espacio. Para el proveedor de esas marcas, esto es una mala noticia: su producto deja de existir en tu tienda sin oportunidad de defenderse."
- Veredicto: "La cadena gana en margen, el proveedor pierde categóricamente y el cliente se va incómodo. Es una decisión unilateral con efectos colaterales."

**Opción B - Pago por venta (Nivel 3)**
- Disponibilidad: Igual — "Igual, 96%"
- Días de inventario: Baja — "Baja de 45 a 38 días"
- Espacio en góndola: Igual — "Igual"
- Tiempo de entrega: Igual — "Igual"
- Costo de transporte: Igual — "Igual"
- Costo operativo: Igual — "Igual"
- Ventas: Igual — "Igual"
- Margen: Baja — "-3% (proveedor cobra más por la flexibilidad)"
- Shopper: Igual — "Igual"
- Cadena: "Mantienes todas las referencias y liberas plata congelada. Pero el proveedor te sube el precio para cubrir el riesgo. Ganas un poquito menos por unidad pero recuperas mucho capital."
- Proveedor: "Ahora carga con el riesgo del inventario sin vender. Sube el precio para protegerse, pero gana visibilidad de qué se vende y dónde. Es un sacrificio inicial a cambio de información."
- Veredicto: "Empiezan a colaborar, pero todavía no es un pacto equilibrado. La cadena traslada riesgo al proveedor y este se cubre subiendo precio. Es un primer paso de confianza, no el destino final."

**Opción C - Planear surtido juntos (Nivel 4)**
- Disponibilidad: Sube — "Sube de 96% a 98%"
- Días de inventario: Baja — "Baja de 45 a 28 días"
- Espacio en góndola: Sube — "Optimizado por categoría"
- Tiempo de entrega: Baja — "Baja de 7 a 4 días"
- Costo de transporte: Baja — "-12%"
- Costo operativo: Sube — "+3% (reuniones mensuales y análisis conjunto)"
- Ventas: Sube — "+9%"
- Margen: Sube — "+19%"
- Shopper: Sube — "Sube"
- Cadena: "La cadena aporta datos de comportamiento del shopper, el proveedor aporta conocimiento de su categoría. Las decisiones son compartidas, así que ambos las defienden y las ejecutan. Liberas plata, vendes más y el cliente encuentra todo."
- Proveedor: "Participa en las decisiones de qué se queda y qué sale, así no pierde su espacio sin opción a defenderse. Optimiza su portafolio, reduce devoluciones, baja costos y vende más."
- Veredicto: "Ganan los dos al máximo. Esto es planeación conjunta de surtido: ambos analizan, ambos deciden, ambos responden por los resultados. Es uno de los pilares más maduros del OSA colaborativo."

#### Estación 4: Caja registradora

**Contexto:** En las cajas tienes 24 productos de impulso: chocolatinas, chicles, pilas, revistas. Solo 8 de cada 10 veces el cliente los encuentra disponibles, porque solo se reponen al cambio de turno. Estos productos representan el 6% de cada compra y son los que más margen dejan.

**Opción A - Reponedor dedicado (Nivel 1)**
- Disponibilidad: Sube — "Sube de 82% a 93%"
- Días de inventario: Baja — "Baja de 6 a 4 días"
- Espacio en góndola: Igual — "Igual"
- Tiempo de entrega: Igual — "Igual"
- Costo de transporte: Igual — "Igual"
- Costo operativo: Sube — "+11% (sueldo del reponedor)"
- Ventas: Sube — "+15% en impulso"
- Margen: Sube — "+18%"
- Shopper: Sube — "Sube"
- Cadena: "La cadena gana mucho porque el impulso es de los productos con más margen. El sueldo del reponedor se paga solo con las ventas extras."
- Proveedor: "Vende un poco más, pero sin enterarse. Para él es una venta normal, no hay nada distinto en la relación."
- Veredicto: "La cadena resuelve sola y gana. El proveedor se beneficia indirectamente pero queda fuera de la jugada. Buena solución, pero no aprovecha el potencial colaborativo."

**Opción B - Marca líder gestiona toda la zona (Nivel 2 - DELEGACIÓN, no colaboración)**
- Disponibilidad: Sube — "Sube de 82% a 96%"
- Días de inventario: Baja — "Baja de 6 a 3 días"
- Espacio en góndola: Sube — "Optimizado por la marca"
- Tiempo de entrega: Baja — "Reposición diaria"
- Costo de transporte: Igual — "Asumido por el proveedor"
- Costo operativo: Baja — "-8% (lo asume el proveedor)"
- Ventas: Sube — "+22% en impulso"
- Margen: Sube — "+25%"
- Shopper: Sube — "Sube"
- Cadena: "No tiene que preocuparse más por las cajas: la marca repone, decide variedad, asume el riesgo y maximiza la venta. La cadena cobra por el espacio y por la venta. Pero pierde control de la zona más rentable."
- Proveedor: "Tiene control total de la mejor zona de la tienda. Decide qué exhibir según ventas reales, optimiza su portafolio y vende mucho más. Asume costos pero captura todo el valor."
- Veredicto: "Esto no es colaboración, es delegación: una parte deja todo en manos de la otra a cambio de un acuerdo. Funciona, pero es un trato transaccional, no un trabajo en equipo. Si la marca cambia de estrategia, la cadena queda expuesta."

**Opción C - Comité conjunto (Nivel 4)**
- Disponibilidad: Sube — "Sube de 82% a 94%"
- Días de inventario: Baja — "Baja de 6 a 3 días"
- Espacio en góndola: Sube — "Optimizado por datos"
- Tiempo de entrega: Baja — "Reposición ajustada"
- Costo de transporte: Igual — "Igual"
- Costo operativo: Sube — "+5% (gestión del comité y software)"
- Ventas: Sube — "+19% en impulso"
- Margen: Sube — "+21%"
- Shopper: Sube — "Sube"
- Cadena: "Mantiene control de la zona pero suma la inteligencia del proveedor. Ambos ven los datos en tiempo real y ajustan juntos. Mejor disponibilidad, mejor margen y aprendizaje compartido."
- Proveedor: "Participa en decisiones reales, no solo despacha. Aporta su conocimiento de categoría y aprende del comportamiento de compra en esta tienda. Vende más y mejora su portafolio en otras tiendas también."
- Veredicto: "Ganan los dos sin que ninguno pierda control. Esta es la diferencia entre delegar (opción B) y colaborar: aquí ambos siguen siendo dueños de la decisión, solo que la toman con mejor información."

#### Estación 5: Bodega

**Contexto:** Una revisión muestra algo curioso: en el 7% de las veces que un cliente no encuentra un producto, ese producto SÍ está en la bodega de la tienda, solo que nadie lo subió a la góndola. El equipo repone en horarios fijos (3 veces al día) y por costumbre, no mirando qué se está acabando.

**Opción A - Reposición por alerta cada hora (Nivel 1)**
- Disponibilidad: Sube — "Sube de 90% a 95%"
- Días de inventario: Igual — "Igual"
- Espacio en góndola: Sube — "Siempre se ve llena"
- Tiempo de entrega: Igual — "No aplica"
- Costo de transporte: Igual — "Igual"
- Costo operativo: Sube — "+9% (mano de obra extra)"
- Ventas: Sube — "+9% (lo que estaba en bodega ahora se vende)"
- Margen: Sube — "+16%"
- Shopper: Sube — "Sube"
- Cadena: "Resolviste el problema sin comprar nada nuevo: el inventario que ya tenías ahora sí llega al cliente. Plata dormida que ahora se convierte en ventas."
- Proveedor: "Vende más porque la cadena exhibe más, pero no se enteró de nada. Para él es solo más volumen."
- Veredicto: "La cadena gana mucho con un cambio interno. El proveedor se beneficia sin saberlo. Buena decisión, pero deja la oportunidad colaborativa intacta."

**Opción B - Reorganizar bodega (Nivel 1)**
- Disponibilidad: Sube — "Sube de 90% a 93%"
- Días de inventario: Baja — "Baja de 14 a 12 días"
- Espacio en góndola: Igual — "Igual"
- Tiempo de entrega: Igual — "Igual"
- Costo de transporte: Igual — "Igual"
- Costo operativo: Baja — "-3% (menos tiempo de búsqueda)"
- Ventas: Sube — "+4%"
- Margen: Sube — "+6%"
- Shopper: Sube — "Sube"
- Cadena: "Ahora se reabastece más rápido y se cometen menos errores. Inversión mínima, mejora rápida. Pero la mejora también es limitada porque los horarios siguen siendo los mismos."
- Proveedor: "Sin cambios. Sigue despachando como siempre y vendiendo lo mismo."
- Veredicto: "Ganancia rápida y barata, pero sin colaboración. El problema de fondo (los horarios fijos de reposición) sigue ahí."

**Opción C - Sincronizar bodega y producción (Nivel 4)**
- Disponibilidad: Sube — "Sube de 90% a 96%"
- Días de inventario: Baja — "Baja de 14 a 6 días"
- Espacio en góndola: Sube — "Optimizado"
- Tiempo de entrega: Baja — "Baja de 4 a 2 días"
- Costo de transporte: Sube — "+10% (más entregas frecuentes)"
- Costo operativo: Sube — "+6% (software y gestión conjunta)"
- Ventas: Sube — "+12%"
- Margen: Sube — "+18%"
- Shopper: Sube — "Sube"
- Cadena: "Libera espacio de bodega que se vuelve venta o experiencia. Reduce capital congelado a la mitad. La inversión está en software y en tiempo de gestión, pero la rentabilidad sube significativamente."
- Proveedor: "Planifica producción con datos reales, optimiza rutas, reduce devoluciones. Comparte el riesgo de quiebre pero también el beneficio de la mejora en venta. Su relación con esta cadena se vuelve mucho más estratégica."
- Veredicto: "Ganan los dos. La diferencia con la opción anterior es que aquí no es la cadena resolviendo sola, sino los dos jugando como un equipo. Requiere confianza y tecnología, pero la rentabilidad combinada es la más alta."

---

## 10. Requisitos técnicos del hardware

### 10.1 Pantalla touch recomendada

| Característica | Requerido | Recomendado |
|---|---|---|
| Tamaño | 32" mínimo | 43-55" |
| Resolución | 1920×1080 | 4K (3840×2160) |
| Tecnología táctil | Capacitiva multitouch | Capacitiva multitouch (10 puntos) |
| Brillo | 350 nits | 500+ nits (si está cerca de luz natural) |
| Orientación | Horizontal | Horizontal |
| Tasa de refresco | 60 Hz | 60 Hz |

### 10.2 Computador de control

| Componente | Mínimo | Recomendado |
|---|---|---|
| Procesador | Intel i5 8va gen / Ryzen 5 | Intel i7 / Ryzen 7 |
| RAM | 8 GB | 16 GB |
| GPU | Integrada moderna | Dedicada (GTX 1650+) |
| Almacenamiento | SSD 256 GB | SSD 512 GB |
| Sistema Operativo | Windows 10/11 | Windows 11 |
| Conectividad | HDMI/USB-C, USB para touch | Mismo |

### 10.3 Configuración de Unity

| Setting | Valor recomendado |
|---|---|
| Versión Unity | 2022.3 LTS o superior |
| Render Pipeline | URP (Universal Render Pipeline) |
| Build target | Windows Standalone |
| Resolución por defecto | Igual a la pantalla (Fullscreen) |
| Vsync | On |
| Frame rate target | 60 fps |
| Quality settings | Medium (suficiente, evita ventiladores ruidosos) |

### 10.4 Modo kiosco

La aplicación debe ejecutarse en modo kiosco:
- Inicio automático con Windows (configurar en `shell:startup` o tarea programada)
- Sin barra de tareas visible
- Sin acceso al cursor del mouse (Cursor.visible = false; Cursor.lockState = CursorLockMode.Confined)
- Bloqueo de combinaciones de teclado peligrosas (Alt+F4, Win, Ctrl+Alt+Del bloqueable mediante kiosk launcher externo)

---

## 11. Pruebas y casos de validación

### 11.1 Casos de prueba funcionales

| ID | Caso | Resultado esperado |
|---|---|---|
| F01 | Tocar pantalla en estado Attract | Transición a Intro |
| F02 | Tocar hot point en mapa | Transición a Situation con datos correctos |
| F03 | Tocar opción A | Feedback A se muestra correctamente |
| F04 | Tocar "Volver" en cualquier pantalla | Regresa al estado anterior correcto |
| F05 | Inactividad de 60 segundos | Reset automático a Attract |
| F06 | Completar 5 estaciones | Pantalla Summary se muestra |
| F07 | Tocar la misma estación 2 veces | Re-entra y la marca como visitada |
| F08 | KPI con dirección "Sube" y favorable | Flecha verde hacia arriba |
| F09 | KPI con dirección "Sube" y desfavorable | Flecha roja hacia arriba |
| F10 | Shopper "Baja" | Cara triste con fondo rojo claro |
| F11 | Nivel de colaboración 4 | 4 segmentos azules llenos |

### 11.2 Casos de prueba de contenido

Cada combinación de estación × opción (5 × 3 = 15) debe verificarse manualmente para confirmar que:
- El contexto coincide con la documentación
- Las 3 opciones son las correctas
- Cada KPI tiene la dirección correcta y el texto descriptivo correcto
- Los 3 textos de feedback (cadena, proveedor, veredicto) están bien
- El nivel de colaboración es el correcto

### 11.3 Pruebas de usabilidad recomendadas

Antes del lanzamiento, probar la atracción con al menos 5 usuarios de cada perfil:
- Estudiantes de bachillerato (15-18 años)
- Estudiantes universitarios
- Dueños de MiPymes
- Ejecutivos de empresas grandes
- Adultos mayores sin formación técnica

Durante las pruebas, observar:
- Cuánto tiempo dedica el usuario a cada pantalla
- Si comprende el lenguaje
- Si llega a la "moraleja" pedagógica al menos en una de las estaciones
- Si encuentra la pantalla intuitiva o se pierde

---

## 12. Anexos

### 12.1 Referencias visuales

- Simulador interactivo construido en conversación con LOGYCA (referencia de diseño)
- Plantilla Excel de contenido completo (`Plantilla_Experiencia_LOGYCA_COLDEX_OSA_Colaborativo.xlsx`)

### 12.2 Convenciones de naming

- Nombres de archivos de assets: `01_Carnes`, `02_Panaderia`, etc.
- Nombres de prefabs: PascalCase con prefijo `UI_` (`UI_KPICard`, `UI_DecisionButton`)
- Variables públicas serializadas: snake_case en español si son texto editorial, camelCase si son técnicas

### 12.3 Decisiones de diseño documentadas

- **Por qué solo 9 KPIs y no más:** mantener la pantalla legible en touch sin scroll excesivo
- **Por qué 4 niveles de colaboración:** distinción clara entre "tradicional" (1-2) y "colaborativo" (3-4) con punto medio identificable
- **Por qué eliminamos "Costo logístico total":** redundante con "Margen" y duplicaba información
- **Por qué el feedback es tripartito (cadena/proveedor/veredicto):** el mensaje pedagógico central es que ambos lados deben ganar, así que el feedback debe mostrar las dos perspectivas

### 12.4 Glosario para el desarrollador

| Término | Significado |
|---|---|
| OSA | On-Shelf Availability. Disponibilidad del producto en góndola |
| COLDEX | Estudio colaborativo de LOGYCA del cual surge esta atracción |
| LOGYCA Lab | Centro de experiencia de LOGYCA donde estará la atracción |
| Shopper | Comprador / cliente final que va al supermercado |
| Hot point | Punto interactivo dentro del mapa del supermercado |
| KPI | Key Performance Indicator. Indicador de desempeño |
| Feedback narrativo | Texto que explica las consecuencias de una decisión |

### 12.5 Roadmap de mejoras futuras

**Fase 2 (post-piloto):**
- Agregar más estaciones (Frutería, Bebidas, Cuidado personal)
- Animaciones de transición más ricas
- Sonido ambiente del supermercado y feedback sonoro al elegir
- Modo "presentación" para charlas con avance manual del facilitador
- Analytics: registrar qué decisiones eligen los visitantes para reportes a LOGYCA

**Fase 3 (avanzada):**
- Modo multijugador: dos personas comparan sus decisiones lado a lado
- Integración con CRM/registro: el visitante deja su correo y recibe un reporte personalizado
- Versión web/móvil para uso fuera del Lab

---

**Fin del documento.**

Para preguntas sobre este documento, contactar al equipo de LOGYCA Lab COLDEX.
