/** @type {import('tailwindcss').Config} */
export default {
  content: [
    './components/**/*.{vue,js,ts}',
    './layouts/**/*.{vue,js,ts}',
    './pages/**/*.{vue,js,ts}',
    './plugins/**/*.{js,ts}',
    './app.{vue,js,ts}'
  ],
  theme: {
    extend: {
      colors: {
        // FoodBev SETA Official Brand Colors (2024 Corporate Identity Manual)
        'foodbev': {
          'blue': '#221F72',      // Primary Blue (Pantone 2756 C) - RGB: R34 G31 B114
          'red': '#E31B23',       // Primary Red (Pantone 1797 C) - RGB: R227 G27 B35
          'platinum': '#D3D6D8',  // Metallic Platinum (Pantone 427 C) - RGB: R211 G214 B216
          'yellow': '#F5ED60',    // Dandelion Yellow (Pantone 394 C) - RGB: R245 G237 B96
          'nickel': '#8D8E8C',    // Nickel (Pantone 7539 C) - RGB: R171 G175 B166
          'black': '#151922',     // Cool Black (Pantone Black 6 C) - RGB: R0 G0 B0
        },
      },
      fontFamily: {
        'sans': ['Nunito', 'system-ui', 'sans-serif'],
      },
    },
  },
  plugins: [],
}

