# TODOs

## Features

- Decide PowerBeam vs random meteor/lightning/bombardment/charge-shot/doomsday rocket etc
- Retarget surgery target from finger to shoulder
- Decide on arm's fist melee verb or custom punch verb with thumbprint wound type etc lol
- Check Patch_RewardPools xpath isn't too aggressive
- Evaluate whether `Scripts/test-windows.sh` is still necessary or the suite can
  run natively with `dotnet test Tests/1.6/ArchotechThumb.Tests.csproj` — the idiomatic
  pattern BetterTradersGuild uses (its CLAUDE.md warns the Windows-interop script
  corrupts shared `obj/` incremental state; ArchotechAndroidHardware verified
  native runs work and dropped the script, AAH 9bc240f). `DeployToModFolder` is
  already Release-gated here, so Debug `dotnet test` builds won't redeploy.
