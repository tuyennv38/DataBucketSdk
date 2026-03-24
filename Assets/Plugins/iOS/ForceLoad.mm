extern long long _GetInstallTimestampV2(void);

__attribute__((constructor))
static void force_link_timestamp()
{
    (void)_GetInstallTimestampV2;
}
