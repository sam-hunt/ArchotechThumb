# Glossary — Archotech Thumb-specific terminology

These per-language files (`Russian.md`, `Japanese.md`, `ChineseSimplified.md`,
`Korean.md`, `German.md`, `Spanish.md`, `French.md`,
`PortugueseBrazilian.md`) hold everything about a language's translation
that is specific to Archotech Thumb: the localized Workshop title
(`ArchotechThumb_SettingsCategory`), and mod-coined terms and worked
phrasing decisions tied to this mod's own defs (`AT_OrbitalBeamStrike`, the
`AT_ArchotechThumb` hediff/thing/recipe family) once a generation pass
grounds them.

Family-shared, mod-independent findings — LanguageWorker mechanics, style
and corpus rules, and vanilla-grounded common vocabulary (Cancel/Reset
buttons, quality tiers, and so on) — live upstream in the `l10n/` submodule
at `l10n/languages/<Language>.md` (canonical checkout: `~/dev/rimworld-l10n`),
since they apply to any mod in the family, not just this one.

**No language pass has run for this mod yet** (only English ships today), so
every file here is currently a placeholder pointing at the grounding work
still to do — see SKILL.md's "This mod's grounding domain" section for the
archotech-body-part vocabulary that must be grounded against the Core tar
before the first non-English pass. When a future translation pass coins a
term or makes a phrasing decision, record it here. If a pass instead
surfaces a correction to shared mechanics or vocabulary, send that fix
upstream to the l10n repo rather than duplicating it here.
