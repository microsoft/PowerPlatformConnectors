# File Structure for Repository Upload

```
paconn-cli-changes/
├── setup.py                           # MODIFIED: Version update + new dependency
├── README.md                          # MODIFIED: Enhanced documentation
├── CHANGES.md                         # NEW: Summary of all changes
└── paconn/
    ├── __init__.py                    # MODIFIED: Version update
    ├── commands/
    │   ├── validate.py                # MODIFIED: Added script validation
    │   └── params.py                  # MODIFIED: Added --script parameter
    └── operations/
        └── script_validate.py         # NEW: C# script validation engine
```

## Upload Instructions

1. **Copy these files to your repository** maintaining the exact folder structure
2. **Replace existing files** where marked as MODIFIED
3. **Add new files** where marked as NEW
4. **Commit with version tag**: v0.1.0

## Key Changes Summary
- **New Feature**: C# script validation for Power Platform connectors
- **Version**: 0.0.21 → 0.1.0
- **Always Strict**: Script validation enforces all security and best practices
- **Mutual Exclusion**: Can validate either swagger OR script, not both
- **Professional Output**: Consistent formatting across all validation types