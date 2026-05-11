# Prompts de IA para generación de imágenes

**Proyecto:** LOGYCA COLDEX · OSA Colaborativo
**Uso:** copiar/pegar el bloque del panel que necesites en Midjourney, DALL·E 3, Flux 1 o Stable Diffusion XL. Para todos los casos el sistema debe respetar la paleta y la tipografía descritas en la **§1**.

---

## 1. Identidad de marca y guía de estilo

### 1.1 Paleta primaria (obligatoria)

| Uso | Color | Hex | Notas |
|---|---|---|---|
| Naranja LOGYCA (acento principal) | 🟧 | `#F5601E` | acentos, hotspots, decisión desacertada |
| Teal LOGYCA LAB | 🟢 | `#28AA94` | check, decisión acertada, segmentos activos |
| Azul oscuro COLDEX | 🔵 | `#042C53` | acentos institucionales |
| Texto principal | ⚫ | `#4A4A48` | títulos, cuerpo |
| Texto secundario | ⚪ | `#8E8E8A` | subtítulos, captions |
| Background hueso | ⬜ | `#FAFAF7` | fondo general |
| Background card | ⬜ | `#F1EFE8` | cards secundarios |

### 1.2 Paleta semántica (cards y estados)

| Estado | Fondo | Texto |
|---|---|---|
| Acertado | `#DBF0EA` (teal claro) | `#0F6B57` |
| Desacertado | `#FCEBE3` (naranja claro) | `#B73E10` |
| Neutro / info | `#E6F1FB` (azul claro) | `#0C447C` |

### 1.3 Tipografía

- Familia: **Inter** o **Montserrat** (sans-serif geométrica, gratuita en Google Fonts).
- Pesos: Regular 400, Medium 500, Bold 700.
- Estilo: minimalista, alta legibilidad, mucho aire entre líneas.

### 1.4 Mood y dirección de arte

- Estética **flat, moderna, editorial** — nada skeumórfico.
- Composición tipo dashboard / data-design, generosa en espacio en blanco.
- Sombras muy sutiles (offset 0–4 px, blur 8–16 px, opacidad 5–10%).
- Esquinas redondeadas 12–20 px en cards, 50% en pills/badges.
- Sin gradientes pesados — máximo gradientes tonales del 5–10%.
- Iconos estilo outline (Tabler / Phosphor), trazo 1.5–2 px, jamás emojis del sistema.
- Cero stock photos genéricas. Si aparecen ilustraciones, estilo vector plano.

---

## 2. Prompt base (prefijo común)

> *Pega este texto al inicio de cualquier prompt si el modelo lo permite, o úsalo como "style reference".*

```
UI design mockup, flat modern editorial style, generous whitespace,
brand palette strictly limited to: vivid orange #F5601E, fresh teal #28AA94,
deep navy #042C53, charcoal text #4A4A48, off-white background #FAFAF7.
Typography: Inter Bold for titles, Inter Regular for body, high contrast.
Subtle shadows under cards (8-16px blur, 5-10% opacity).
Rounded corners (12-20px on cards, fully rounded on pills).
Outline icons (1.5-2px stroke), no emojis, no gradients,
no stock photography. Designed for a 1920x1080 touch screen.
Negative prompt: cluttered, neon, drop shadows, glossy, 3D bevels,
default windows UI, lorem ipsum, watermark, low resolution.
```

---

## 3. Prompts por panel

### 3.1 Panel Attract — pantalla de atracción

**Objetivo:** captar la atención del visitante para que toque la pantalla.

```
Full-screen attractor UI for a museum-grade touch installation by LOGYCA LAB
and COLDEX, 1920x1080 landscape. Centered hero typography reading
"MUEVE EL DATO," on one line and "MUEVE LA GÓNDOLA" on the next, in
Inter Bold 96pt, color #4A4A48. Below the title in 32pt Inter Regular
the line "Toca para empezar" in color #F5601E (LOGYCA orange), gently
pulsing. Subtle abstract background: a faint isometric line-art of a
supermarket aisle in single-color stroke (#28AA94, 20% opacity), drifting
diagonally. Tiny LOGYCA LAB logo (lightbulb with circuit) in the top-right
corner. Off-white background #FAFAF7. Generous margins (128px sides),
absolutely no other UI elements. Mood: aspirational, clean, inviting.
--ar 16:9 --style raw --v 6
```

**Variantes a probar:**
- Cambiar el line-art por "supermarket cart silhouette" o "shelf grid pattern".
- Modo oscuro: background `#042C53` con tipografía blanca.

---

### 3.2 Panel Map — frame 01 (vista general)

**Objetivo:** mockup del overlay sobre la escena 3D del mall, con hotspots pulsantes y RondaCounter abajo.

> *Nota: la escena 3D la genera Unity; este prompt es para el **overlay UI** que va encima.*

```
HUD UI overlay for a 3D supermarket isometric view, 1920x1080. Top bar:
dark olive #4A4A48 strip 90px tall, semi-transparent (90% opacity), with
the label "HUD" + "Siempre visible" in white on the left (small caps,
12pt with 22pt heading), and on the right three indicator chips spaced
evenly: "OSA · 92%" in teal #28AA94, "INV · OK" in white, "SOS · 38%"
in orange #F5601E. Center area: five circular pulsing hotspot rings,
orange #F5601E, 80px diameter, scattered organically over imagined
supermarket zones. One ring (top-center) is larger (110px) with a
floating pill label above it reading "PRÓXIMA" in white text on orange
#F5601E rounded-fully background. Bottom-left: a dark pill (#3A3A38)
80% opacity, 360x60px, reading "● RONDA 2 DE 5 · acercando cámara al
hotspot" in white, where the bullet is orange #F5601E. Transparent
canvas background (the 3D scene will go behind). Mood: cinematic,
guiding, professional. No clutter.
--ar 16:9 --style raw --v 6
```

---

### 3.3 Panel Situation — frames 02 + 03 (acercamiento + DATO/pregunta)

**Objetivo:** card flotante con DATO + pregunta + 3 botones, sobre la escena 3D acercada al hotspot.

```
Touch-screen UI overlay, 1920x1080. The bottom 45% of the screen is occupied
by a large floating card with rounded corners (20px) and a very soft shadow.
Card background: off-white #FAFAF7. At the top-left of the card a small
pill chip with text "DATO" in white on orange #F5601E background, fully
rounded, 16pt Inter Bold uppercase. To the right of the chip, a single
paragraph of italic body text (22pt Inter Regular, color #4A4A48) reading
"La frutería tiene mermas del 12% esta semana. La fruta que se daña es la
que está al fondo del exhibidor." Below it, an 8px-tall horizontal divider
in light gray #E5E3DC. Then the question "¿Qué deberías hacer primero?"
in 28pt Inter Bold #4A4A48. Below it three stacked option buttons,
full-width, 90px tall each, 12px corner radius:
  - First button: solid teal #28AA94 background, white text "A · Rotar fruta: pasar la del fondo al frente", 22pt Inter Medium.
  - Second button: white background #FFFFFF, 1px border #E5E3DC, dark text "B · Bajar el precio de la fruta del fondo".
  - Third button: same neutral style "C · Pedir menos fruta al próximo despacho".
Above the card, floating on the (imaginary) 3D scene: a centered teal
pill "MERCADERISTA EN HOTSPOT" in 14pt Inter Bold uppercase, white on
#28AA94 background, fully rounded. Also a hollow orange #F5601E rectangle
outline (3px stroke, 20px corner radius) selecting an imagined gondola
area to the left of where the mercaderista pill points. The 3D scene
above is implied / mocked as a soft pastel grocery aisle blur with
warm wood tones, dimmed to 80% so the card pops. Top: the persistent
dark HUD bar with three indicators (OSA · 92%, INV · OK, SOS · 38%).
Mood: focused, instructive, slightly cinematic.
--ar 16:9 --style raw --v 6
```

**Variante:** sin la escena 3D atrás, solo el card sobre fondo blanco para usar como prefab de Figma.

---

### 3.4 Panel Feedback — frame 04 (decisión + veredicto)

**Objetivo:** mockup de los dos posibles resultados (acertado / desacertado) lado a lado, slide 9.

```
Side-by-side comparison UI mockup for a touch screen, 1920x1080.
Two equal-width panels separated by 24px gap.

LEFT PANEL (decisión desacertada):
- Top chip "DECISIÓN DESACERTADA" with a small circle icon containing
  a white X cross, background orange #F5601E, white text, 18pt Inter
  Bold uppercase.
- Below in 24pt italic: "Pedir menos fruta al próximo despacho"
- Below that, a hollow rectangle outline (3px stroke #F5601E) framing
  an imagined empty fruit shelf, with a small floating pill above
  reading "FRUTERÍA SIN ATENDER" in white on #F5601E.
- Bottom card "PORQUÉ" (16pt uppercase tracking) followed by body
  text "Pedir menos baja la merma pero también el OSA y el espacio
  en góndola. La competencia ocupa el aire que cediste."
- Three mini indicators in a row at the very bottom:
    OSA -6% (red), INV -3u (red), SOS -4% (red)
  each label in small caps uppercase, value in 32pt Inter Bold,
  color #B73E10 (orange darkened).
- Background card #FCEBE3 (light orange tint).

RIGHT PANEL (decisión acertada):
- Mirror layout, color swapped to teal:
- Chip "DECISIÓN ACERTADA" with white check mark on #28AA94.
- Title: "Rotar fruta: pasar la del fondo al frente"
- Hollow rectangle outline in teal #28AA94, pill above reading
  "FRUTERÍA ROTADA" in white on #28AA94.
- "PORQUÉ": "Rotar la fruta protege OSA sin sacrificar inventario.
  La merma baja porque la fruta vieja sale antes de dañarse."
- Three mini indicators: OSA +4% (green), INV → 0 (neutral gray),
  SOS +2% (green), color #0F6B57.
- Background card #DBF0EA (light teal tint).

Top of both panels: persistent dark HUD bar reflecting the new state
(updated values + "antes 92%" tiny subtitle in lighter gray).
Off-white background #FAFAF7 between them. Mood: educational,
comparative, clean.
--ar 16:9 --style raw --v 6
```

**Variante "una sola decisión":** generar solo el panel acertado o el desacertado, en orientación 16:9.

---

### 3.5 Panel Summary — cierre y reflexión

```
Closing screen mockup for a 1920x1080 touch installation. Off-white
background #FAFAF7. Centered hero title in 56pt Inter Bold #4A4A48:
"¡Recorriste el supermercado!". Below it a horizontal HUD bar 1200px
wide, 100px tall, dark olive #4A4A48 90% opacity, containing the three
final indicators in their respective colors with delta arrows and
"antes X%" subtitles. Below the HUD bar, a single rounded card
(900x180px, #F1EFE8 background, 20px radius) showing in two stacked
lines: "COLABORADOR" in 40pt Inter Bold uppercase #28AA94, and
"Promedio nivel de colaboración: 3.2" in 22pt Inter Regular #4A4A48.
Below the card, a centered single-line quote in 26pt Inter italic
#8E8E8A: "No se trata del indicador correcto, sino de saber qué se
cede al priorizar otro." Bottom of screen: one prominent button,
fully rounded, 64pt tall, orange #F5601E background, white 24pt
Inter Bold text reading "Empezar de nuevo". LOGYCA LAB logo in
the top-right corner. Mood: reflective, optimistic, conclusive.
--ar 16:9 --style raw --v 6
```

---

## 4. Prompts para assets individuales

### 4.1 Hotspot pulsante (sprite)

```
Single isolated UI asset for game/web, 512x512 transparent PNG.
Two concentric circles centered on canvas: inner solid circle
220px diameter, vivid orange #F5601E. Outer ring 380px diameter,
hollow stroke 6px, same orange, 60% opacity. A subtle outermost
ring 480px diameter, 2px stroke, 25% opacity. Tiny white dot
24px in the center. No background, no shadow, transparent canvas.
Style: flat, geometric, modern UI element.
--ar 1:1
```

### 4.2 Pill "PRÓXIMA" y "MERCADERISTA EN HOTSPOT"

```
Two isolated UI pill labels on transparent canvas.
First pill: fully rounded rectangle 360x80px, background solid
orange #F5601E, no border, white text "PRÓXIMA" centered, Inter Bold
28pt uppercase tracked 8%.
Second pill: same shape, background solid teal #28AA94, white text
"MERCADERISTA EN HOTSPOT" centered, same typography.
Output as two separate PNG with transparent background.
--ar 4:1
```

### 4.3 Iconos de sección (5 estaciones)

```
Set of 5 outline icons for supermarket categories, monochrome on
transparent background, stroke 2px, color #4A4A48. 256x256 each,
arranged in a horizontal strip. Style: Tabler Icons / Phosphor —
clean, geometric, friendly:
  1. Apple / pineapple cluster (frutería)
  2. Meat cleaver with steak (carnes y lácteos)
  3. Bread loaf with steam wave (panadería)
  4. Oil bottle row (abarrotes)
  5. Cardboard box with arrow (bodega)
No fills, no shadows, no color other than #4A4A48.
--ar 5:1
```

### 4.4 Iconos KPI (flechas + caras)

```
UI icon set, transparent background, three rows of three icons each.
Row 1 (arrows): up-right diagonal arrow stroke 2.5px in teal #28AA94,
  down-right diagonal arrow in orange #F5601E, horizontal dash in
  neutral gray #8E8E8A. All in 128x128 squares.
Row 2 (faces): happy smile on teal background tint #DBF0EA,
  neutral straight mouth on gray tint #F1EFE8, sad frown on
  orange tint #FCEBE3. All circles 128px with thin 2px outline.
Row 3 (badges): check mark in white on teal circle #28AA94,
  X cross in white on orange circle #F5601E, exclamation mark
  in white on dark navy #042C53. All circles 96px.
Outline style, geometric, no gradients, no skeuomorphism.
--ar 3:3
```

### 4.5 Background ambient para Attract

```
Abstract decorative background, 1920x1080, off-white #FAFAF7 base.
Faint geometric line art at 8-15% opacity: isometric grid of
supermarket shelves drawn as single-color outlines in teal #28AA94,
trailing diagonally from bottom-left to top-right, fading toward
the corners. Tiny abstract data points (small dots and connecting
lines, orange #F5601E, 5% opacity) scattered like a sparse network
graph. Empty center area (60% of canvas) reserved for typography.
Minimalist, editorial, vector-flat aesthetic.
--ar 16:9 --style raw --v 6
```

### 4.6 Tabla de deltas / fondo card grande

```
UI element on transparent canvas, 1400x600. A single large rounded
card (24px corner radius) with background #FAFAF7 and a very subtle
inner stroke (1px, #E5E3DC). Inside the card, a 3-column grid with
column dividers (1px lines #E5E3DC). Each column has a small header
label in 14pt Inter Bold uppercase tracked 10% color #8E8E8A, and
below a large value in 36pt Inter Bold. Headers and values for
slot: "OSA · +4%" (teal), "INV · → 0" (gray), "SOS · +2%" (teal).
Generous internal padding (32px). Flat, no shadows.
--ar 7:3
```

---

## 5. Notas técnicas

### 5.1 Formato y resolución
- Mockups de panel completo: **1920×1080 PNG** o JPG alta calidad.
- Assets individuales: **PNG con transparencia**, mínimo 2× la resolución de uso (para retina y escalado).
- Importar a Unity como `Texture Type = Sprite (2D and UI)`, `Mesh Type = Tight`, **9-slice** activado en cards y botones.

### 5.2 Cómo combinar con Midjourney
- Añadir al final: `--ar 16:9 --style raw --v 6 --stylize 100`.
- `--stylize 100` baja la creatividad y respeta más los colores.
- Si el modelo "inventa" colores fuera de la paleta, añadir `--no neon, --no gradient, --no pastel`.

### 5.3 Cómo combinar con DALL·E 3 / GPT
- Pegar el prompt completo (incluyendo el §2 base).
- Pedir "exactamente la paleta hex listada, no inventes colores".
- Pedir "without rendering any text incorrectly — leave Spanish text intact".

### 5.4 Cómo combinar con Flux 1 / SDXL
- Bajar el CFG a 6–7 para evitar saturación de color.
- Usar `style: editorial UI mockup` o un LoRA de "minimal UI".

### 5.5 Negative prompt unificado (pegable)

```
neon colors, dark mode, drop shadows, glossy 3D bevels, default
windows or macOS chrome, blue Material Design, glassmorphism,
glitter, sparkles, particles, photographic stock imagery, low
resolution, jpeg artifacts, text gibberish, lorem ipsum,
watermark, signature, frames, borders around the canvas.
```

---

## 6. Workflow recomendado

1. Generar primero **§3.1 Attract** y **§3.5 Summary** (los más simples) para validar que la IA respeta la paleta.
2. Una vez la paleta es consistente, generar **§3.2 Map**, **§3.3 Situation**, **§3.4 Feedback**.
3. Después generar los **assets individuales (§4)** que falten en la carpeta `Assets/LOGYCA/UI/` (iconos KPI, caras shopper, hotspots a mayor resolución).
4. Pasar todos los assets generados por un editor vectorial (Figma / Illustrator) para limpiar bordes y ajustar la paleta si la IA se salió por ±5%.

---

**Fin del documento.** Para cualquier prompt nuevo, mantener siempre la paleta de la §1 y el negative prompt unificado de la §5.5.
