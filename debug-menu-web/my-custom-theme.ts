
import type { CustomThemeConfig } from '@skeletonlabs/tw-plugin';

export const myCustomTheme: CustomThemeConfig = {
    name: 'my-custom-theme',
    properties: {
        // =~= Theme Properties =~=
        "--theme-font-family-base": `Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, 'Noto Sans', sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol', 'Noto Color Emoji'`,
        "--theme-font-family-heading": `Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, 'Noto Sans', sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol', 'Noto Color Emoji'`,
        "--theme-font-color-base": "0 0 0",
        "--theme-font-color-dark": "255 255 255",
        "--theme-rounded-base": "12px",
        "--theme-rounded-container": "8px",
        "--theme-border-base": "1px",
        // =~= Theme On-X Colors =~=
        "--on-primary": "255 255 255",
        "--on-secondary": "0 0 0",
        "--on-tertiary": "0 0 0",
        "--on-success": "0 0 0",
        "--on-warning": "0 0 0",
        "--on-error": "255 255 255",
        "--on-surface": "255 255 255",
        // =~= Theme Colors  =~=
        // primary | #5e6bc9 
        "--color-primary-50": "231 233 247", // #e7e9f7
        "--color-primary-100": "223 225 244", // #dfe1f4
        "--color-primary-200": "215 218 242", // #d7daf2
        "--color-primary-300": "191 196 233", // #bfc4e9
        "--color-primary-400": "142 151 217", // #8e97d9
        "--color-primary-500": "94 107 201", // #5e6bc9
        "--color-primary-600": "85 96 181", // #5560b5
        "--color-primary-700": "71 80 151", // #475097
        "--color-primary-800": "56 64 121", // #384079
        "--color-primary-900": "46 52 98", // #2e3462
        // secondary | #cc89ec 
        "--color-secondary-50": "247 237 252", // #f7edfc
        "--color-secondary-100": "245 231 251", // #f5e7fb
        "--color-secondary-200": "242 226 250", // #f2e2fa
        "--color-secondary-300": "235 208 247", // #ebd0f7
        "--color-secondary-400": "219 172 242", // #dbacf2
        "--color-secondary-500": "204 137 236", // #cc89ec
        "--color-secondary-600": "184 123 212", // #b87bd4
        "--color-secondary-700": "153 103 177", // #9967b1
        "--color-secondary-800": "122 82 142", // #7a528e
        "--color-secondary-900": "100 67 116", // #644374
        // tertiary | #0de4e7 
        "--color-tertiary-50": "219 251 251", // #dbfbfb
        "--color-tertiary-100": "207 250 250", // #cffafa
        "--color-tertiary-200": "195 248 249", // #c3f8f9
        "--color-tertiary-300": "158 244 245", // #9ef4f5
        "--color-tertiary-400": "86 236 238", // #56ecee
        "--color-tertiary-500": "13 228 231", // #0de4e7
        "--color-tertiary-600": "12 205 208", // #0ccdd0
        "--color-tertiary-700": "10 171 173", // #0aabad
        "--color-tertiary-800": "8 137 139", // #08898b
        "--color-tertiary-900": "6 112 113", // #067071
        // success | #21cb15 
        "--color-success-50": "222 247 220", // #def7dc
        "--color-success-100": "211 245 208", // #d3f5d0
        "--color-success-200": "200 242 197", // #c8f2c5
        "--color-success-300": "166 234 161", // #a6eaa1
        "--color-success-400": "100 219 91", // #64db5b
        "--color-success-500": "33 203 21", // #21cb15
        "--color-success-600": "30 183 19", // #1eb713
        "--color-success-700": "25 152 16", // #199810
        "--color-success-800": "20 122 13", // #147a0d
        "--color-success-900": "16 99 10", // #10630a
        // warning | #EAB308 
        "--color-warning-50": "252 244 218", // #fcf4da
        "--color-warning-100": "251 240 206", // #fbf0ce
        "--color-warning-200": "250 236 193", // #faecc1
        "--color-warning-300": "247 225 156", // #f7e19c
        "--color-warning-400": "240 202 82", // #f0ca52
        "--color-warning-500": "234 179 8", // #EAB308
        "--color-warning-600": "211 161 7", // #d3a107
        "--color-warning-700": "176 134 6", // #b08606
        "--color-warning-800": "140 107 5", // #8c6b05
        "--color-warning-900": "115 88 4", // #735804
        // error | #d21919 
        "--color-error-50": "248 221 221", // #f8dddd
        "--color-error-100": "246 209 209", // #f6d1d1
        "--color-error-200": "244 198 198", // #f4c6c6
        "--color-error-300": "237 163 163", // #eda3a3
        "--color-error-400": "224 94 94", // #e05e5e
        "--color-error-500": "210 25 25", // #d21919
        "--color-error-600": "189 23 23", // #bd1717
        "--color-error-700": "158 19 19", // #9e1313
        "--color-error-800": "126 15 15", // #7e0f0f
        "--color-error-900": "103 12 12", // #670c0c
        // surface | #102051 
        "--color-surface-50": "219 222 229", // #dbdee5
        "--color-surface-100": "207 210 220", // #cfd2dc
        "--color-surface-200": "195 199 212", // #c3c7d4
        "--color-surface-300": "159 166 185", // #9fa6b9
        "--color-surface-400": "88 99 133", // #586385
        "--color-surface-500": "16 32 81", // #102051
        "--color-surface-600": "14 29 73", // #0e1d49
        "--color-surface-700": "12 24 61", // #0c183d
        "--color-surface-800": "10 19 49", // #0a1331
        "--color-surface-900": "8 16 40", // #081028

    }
}