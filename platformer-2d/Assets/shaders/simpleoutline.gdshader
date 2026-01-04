shader_type canvas_item;

uniform bool outline_enabled = false;
uniform float outline_size = 2.0;
uniform vec4 outline_color : hint_color = vec4(1.0, 1.0, 1.0, 1.0);

void fragment() {
    vec4 tex = texture(TEXTURE, UV);
    if (outline_enabled) {
        float alpha = tex.a;
        if (alpha < 0.1) {
            float outline = 0.0;
            for (float x = -outline_size; x <= outline_size; x += 1.0) {
                for (float y = -outline_size; y <= outline_size; y += 1.0) {
                    vec2 offset = vec2(x, y) / TEXTURE_PIXEL_SIZE;
                    outline = max(outline, texture(TEXTURE, UV + offset).a);
                }
            }
            if (outline > 0.1) {
                COLOR = outline_color;
                return;
            }
        }
    }
    COLOR = tex;
}