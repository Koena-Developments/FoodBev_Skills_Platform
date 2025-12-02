# FoodBev SETA Branding Implementation

This document outlines the FoodBev SETA branding that has been implemented in the application.

## Brand Colors

The following official FoodBev SETA brand colors have been configured in `tailwind.config.js`:

### Primary Colors
- **FoodBev Blue**: `#221F72` (Pantone 2756 C) - Primary brand color
- **FoodBev Red**: `#E31B23` (Pantone 1797 C) - Secondary brand color

### Secondary Colors
- **Metallic Platinum**: `#D3D6D8` (Pantone 427 C) - Used for backgrounds and accents
- **Dandelion Yellow**: `#F5ED60` (Pantone 394 C) - Used for highlights
- **Nickel**: `#8D8E8C` (Pantone 7539 C) - Used for subtle elements
- **Cool Black**: `#151922` (Pantone Black 6 C) - Used for text

## Typography

The application uses **Nunito** font family as specified in the FoodBev SETA Corporate Identity Manual. The font is loaded from Google Fonts.

## Logo

### Logo File Location
Place the official FoodBev SETA logo file at:
```
FoodBev.UI/public/foodbev-logo.png
```

The logo component (`components/common/Logo.vue`) is configured to:
- Display the logo image if available at `/foodbev-logo.png`
- Fall back to a text-based logo with brand colors if the image is not found
- Support multiple sizes (sm, md, lg, xl)

### Logo Usage
The logo component is used throughout the application in:
- Login page
- Register page
- All layout sidebars (Admin, Candidate, Employer)
- Any other pages that require branding

## Implementation Details

### Tailwind Configuration
All brand colors are available as Tailwind utility classes:
- `bg-foodbev-blue`, `text-foodbev-blue`, `border-foodbev-blue`
- `bg-foodbev-red`, `text-foodbev-red`, `border-foodbev-red`
- `bg-foodbev-platinum`, `text-foodbev-platinum`
- `bg-foodbev-yellow`, `text-foodbev-yellow`
- `bg-foodbev-nickel`, `text-foodbev-nickel`
- `bg-foodbev-black`, `text-foodbev-black`

### Pages Updated
The following pages have been updated with FoodBev SETA branding:
- Login page (`pages/login.vue`)
- Register page (`pages/register.vue`)
- Candidate dashboard (`pages/candidate/index.vue`)
- Admin dashboard (`pages/admin/index.vue`)
- All layout files (admin, candidate, employer)

### Components Updated
- Logo component (`components/common/Logo.vue`)
- All buttons and interactive elements now use brand colors

## Next Steps

1. **Add Official Logo**: Download the official FoodBev SETA logo from their Corporate Identity Manual and place it at `public/foodbev-logo.png`
2. **Review Brand Guidelines**: Ensure all usage complies with the FoodBev SETA Corporate Identity Manual
3. **Test Color Contrast**: Verify that all text meets accessibility standards with the brand colors

## References

- FoodBev SETA Corporate Identity Manual (2024)
- Official website: https://foodbev.co.za

