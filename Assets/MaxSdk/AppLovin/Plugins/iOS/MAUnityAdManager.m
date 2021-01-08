//
//  MAUnityAdManager.m
//  AppLovin MAX Unity Plugin
//

#import "MAUnityAdManager.h"

#define VERSION @"2.1.0"

#ifdef __cplusplus
extern "C" {
#endif
    // life cycle management
    void UnityPause(int pause);
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
#ifdef __cplusplus
}
#endif

@interface MAUnityAdManager()<MAAdDelegate, MAAdViewAdDelegate, MARewardedAdDelegate, ALVariableServiceDelegate>
@property (nonatomic,   weak) ALSdk *sdk;
@property (nonatomic, strong) NSMutableDictionary<NSString *, MAInterstitialAd *> *interstitials;
@property (nonatomic, strong) NSMutableDictionary<NSString *, MARewardedAd *> *rewardedAds;
@property (nonatomic, strong) NSMutableDictionary<NSString *, MAAdView *> *adViews;
@property (nonatomic, strong) NSMutableDictionary<NSString *, NSString *> *adViewPositions;
@property (nonatomic, strong) UIView *safeAreaBackground;
@end

// Internal
@interface UIColor (ALUtils)
+ (nullable UIColor *)al_colorWithHexString:(NSString *)hexString;
@end

@implementation MAUnityAdManager
static NSString *const TAG = @"MAUnityAdManager";
static NSString *ALSerializeKeyValueSeparator;
static NSString *ALSerializeKeyValuePairSeparator;

#pragma mark - Initialization

+ (void)initialize
{
    [super initialize];
    
    ALSerializeKeyValueSeparator = [NSString stringWithFormat: @"%c", 28];
    ALSerializeKeyValuePairSeparator = [NSString stringWithFormat: @"%c", 29];
}

- (instancetype)init
{
    self = [super init];
    if ( self )
    {
        self.interstitials = [NSMutableDictionary dictionaryWithCapacity: 2];
        self.rewardedAds = [NSMutableDictionary dictionaryWithCapacity: 2];
        self.adViews = [NSMutableDictionary dictionaryWithCapacity: 2];
        self.adViewPositions = [NSMutableDictionary dictionaryWithCapacity: 2];
        self.safeAreaBackground = [[UIView alloc] init];
        self.safeAreaBackground.hidden = YES;
        self.safeAreaBackground.backgroundColor = UIColor.clearColor;
        self.safeAreaBackground.translatesAutoresizingMaskIntoConstraints = NO;
        
        UIViewController *rootViewController = [MAUnityAdManager unityViewController];
        [rootViewController.view addSubview: self.safeAreaBackground];
    }
    return self;
}

#pragma mark - Plugin Initialization

- (ALSdk *)initializeSdkWithCompletionHandler:(ALSdkInitializationCompletionHandler)completionHandler
{
    self.sdk = [ALSdk shared];
    self.sdk.variableService.delegate = self;
    [self.sdk setPluginVersion: [@"Max-Unity-" stringByAppendingString: VERSION]];
    self.sdk.mediationProvider = @"max";
    [self.sdk initializeSdkWithCompletionHandler:^(ALSdkConfiguration *configuration)
     {
         // Note: internal state should be updated first
         completionHandler( configuration );
         
         NSString *consentDialogStateStr = @(configuration.consentDialogState).stringValue;
         [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": @"OnSdkInitializedEvent",
                                                        @"consentDialogState": consentDialogStateStr}];
     }];
    
    return self.sdk;
}

#pragma mark - Banners

- (void)createBannerWithAdUnitIdentifier:(NSString *)adUnitIdentifier atPosition:(NSString *)bannerPosition
{
    // Remove banner from the map
    MAAdView *adView = [self retrieveAdViewForAdUnitIdentifier: adUnitIdentifier atPosition: bannerPosition];
    adView.hidden = YES;
    self.safeAreaBackground.hidden = YES;
    
    [adView loadAd];
}

- (void)setBannerBackgroundColorForAdUnitIdentifier:(NSString *)adUnitIdentifier hexColorCode:(NSString *)hexColorCode
{
    // In some cases, black color may get redrawn on each frame update, resulting in an undesired flicker
    if ( [hexColorCode containsString: @"FF000000"] ) hexColorCode = @"FF000001";
    
    UIColor *convertedColor = [UIColor al_colorWithHexString: hexColorCode];
    
    MAAdView *view = [self retrieveAdViewForAdUnitIdentifier: adUnitIdentifier];
    self.safeAreaBackground.backgroundColor = view.backgroundColor = convertedColor;
}

- (void)setBannerPlacement:(nullable NSString *)placement forAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    MAAdView *adView = [self retrieveAdViewForAdUnitIdentifier: adUnitIdentifier];
    adView.placement = placement;
}

- (void)showBannerWithAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    // Remove banner from the map
    MAAdView *view = [self retrieveAdViewForAdUnitIdentifier: adUnitIdentifier];
    self.safeAreaBackground.hidden = NO;
    view.hidden = NO;
    
    [view startAutoRefresh];
}

- (void)hideBannerWithAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    // Remove banner from the map
    MAAdView *view = [self retrieveAdViewForAdUnitIdentifier: adUnitIdentifier];
    view.hidden = YES;
    self.safeAreaBackground.hidden = YES;
    
    [view stopAutoRefresh];
}

- (void)destroyBannerWithAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    // Remove banner from the map
    MAAdView *view = [self retrieveAdViewForAdUnitIdentifier: adUnitIdentifier];
    view.delegate = nil;
    
    [view removeFromSuperview];
    
    [self.adViews removeObjectForKey: adUnitIdentifier];
    [self.adViewPositions removeObjectForKey: adUnitIdentifier];
}

#pragma mark - Interstitials

- (void)loadInterstitialWithAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    MAInterstitialAd *interstitial = [self retrieveInterstitialForAdUnitIdentifier: adUnitIdentifier];
    [interstitial loadAd];
}

- (BOOL)isInterstitialReadyWithAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    MAInterstitialAd *interstitial = [self retrieveInterstitialForAdUnitIdentifier: adUnitIdentifier];
    return [interstitial isReady];
}

- (void)showInterstitialWithAdUnitIdentifier:(NSString *)adUnitIdentifier placement:(NSString *)placement
{
    MAInterstitialAd *interstitial = [self retrieveInterstitialForAdUnitIdentifier: adUnitIdentifier];
    [interstitial showAdForPlacement: placement];
}

#pragma mark - Rewarded

- (void)loadRewardedAdWithAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    MARewardedAd *rewardedAd = [self retrieveRewardedAdForAdUnitIdentifier: adUnitIdentifier];
    [rewardedAd loadAd];
}

- (BOOL)isRewardedAdReadyWithAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    MARewardedAd *rewardedAd = [self retrieveRewardedAdForAdUnitIdentifier: adUnitIdentifier];
    return [rewardedAd isReady];
}

- (void)showRewardedAdWithAdUnitIdentifier:(NSString *)adUnitIdentifier placement:(NSString *)placement
{
    MARewardedAd *rewardedAd = [self retrieveRewardedAdForAdUnitIdentifier: adUnitIdentifier];
    [rewardedAd showAdForPlacement: placement];
}

#pragma mark - Event Tracking

- (void)trackEvent:(NSString *)event parameters:(NSString *)parameters
{
    NSDictionary<NSString *, NSString *> *deserializedParameters = [self deserializeEventParameters: parameters];
    [self.sdk.eventService trackEvent: event parameters: deserializedParameters];
}

#pragma mark - Variable Service

- (void)loadVariables
{
    [self.sdk.variableService loadVariables];
}

- (void)variableService:(ALVariableService *)variableService didUpdateVariables:(NSDictionary<NSString *, id> *)variables
{
    [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": @"OnVariablesUpdatedEvent"}];
}

#pragma mark - Ad Callbacks

- (void)didLoadAd:(MAAd *)ad
{
    NSString *name;
    if ( ad.format == MAAdFormat.banner || ad.format == MAAdFormat.leader )
    {
        name = @"OnBannerAdLoadedEvent";
        [self positionBannerForAdUnitIdentifier: ad.adUnitIdentifier];
        
        // Do not auto-refresh by default if the banner is not showing yet (e.g. first load during app launch and publisher does not automatically show banner upon load success)
        // We will resume auto-refresh in -[MAUnityAdManager showBannerWithAdUnitIdentifier:].
        MAAdView *adView = [self retrieveAdViewForAdUnitIdentifier: ad.adUnitIdentifier];
        if ( adView && [adView isHidden] )
        {
            [adView stopAutoRefresh];
        }
    }
    else if ( ad.format == MAAdFormat.interstitial)
    {
        name = @"OnInterstitialLoadedEvent";
    }
    else if ( ad.format == MAAdFormat.rewarded)
    {
        name = @"OnRewardedAdLoadedEvent";
    }
    else
    {
        [self logInvalidAdFormat: ad.format];
        return;
    }
    
    [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": name,
                                                   @"adUnitId": ad.adUnitIdentifier}];
}

- (void)didFailToLoadAdForAdUnitIdentifier:(NSString *)adUnitIdentifier withErrorCode:(NSInteger)errorCode
{
    if ( !adUnitIdentifier )
    {
        [self log: @"adUnitIdentifier cannot be nil from %@", [NSThread callStackSymbols]];
        return;
    }
    
    NSString *name;
    if ( self.adViews[adUnitIdentifier] )
    {
        name = @"OnBannerAdLoadFailedEvent";
    }
    else if ( self.interstitials[adUnitIdentifier] )
    {
        name = @"OnInterstitialLoadFailedEvent";
    }
    else if ( self.rewardedAds[adUnitIdentifier] )
    {
        name = @"OnRewardedAdLoadFailedEvent";
    }
    else
    {
        [self log: @"invalid adUnitId from %@", [NSThread callStackSymbols]];
        return;
    }
    
    NSString *errorCodeStr = [@(errorCode) stringValue];
    [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": name,
                                                   @"adUnitId": adUnitIdentifier,
                                                   @"errorCode": errorCodeStr}];
}

- (void)didClickAd:(MAAd *)ad
{
    NSString *name;
    if ( ad.format == MAAdFormat.banner || ad.format == MAAdFormat.leader )
    {
        name = @"OnBannerAdClickedEvent";
    }
    else if ( ad.format == MAAdFormat.interstitial )
    {
        name = @"OnInterstitialClickedEvent";
    }
    else if ( ad.format == MAAdFormat.rewarded )
    {
        name = @"OnRewardedAdClickedEvent";
    }
    else
    {
        [self logInvalidAdFormat: ad.format];
        return;
    }
    
    [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": name,
                                                   @"adUnitId": ad.adUnitIdentifier}];
}

- (void)didDisplayAd:(MAAd *)ad
{
    // BMLs do not support [DISPLAY] events in Unity
    if ( ad.format != MAAdFormat.interstitial && ad.format != MAAdFormat.rewarded ) return;
    
    NSString *name;
    if ( ad.format == MAAdFormat.interstitial )
    {
        name = @"OnInterstitialDisplayedEvent";
    }
    else // REWARDED
    {
        name = @"OnRewardedAdDisplayedEvent";
    }
    
    [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": name,
                                                   @"adUnitId": ad.adUnitIdentifier}];
}

- (void)didFailToDisplayAd:(MAAd *)ad withErrorCode:(NSInteger)errorCode
{
    // BMLs do not support [DISPLAY] events in Unity
    if ( ad.format != MAAdFormat.interstitial && ad.format != MAAdFormat.rewarded ) return;
    
    NSString *name;
    if ( ad.format == MAAdFormat.interstitial )
    {
        name = @"OnInterstitialAdFailedToDisplayEvent";
    }
    else // REWARDED
    {
        name = @"OnRewardedAdFailedToDisplayEvent";
    }
    
    NSString *errorCodeStr = [@(errorCode) stringValue];
    [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": name,
                                                   @"adUnitId": ad.adUnitIdentifier,
                                                   @"errorCode": errorCodeStr}];
}

- (void)didHideAd:(MAAd *)ad
{
    // BMLs do not support [HIDDEN] events in Unity
    if ( ad.format != MAAdFormat.interstitial && ad.format != MAAdFormat.rewarded ) return;
    
    NSString *name;
    if ( ad.format == MAAdFormat.interstitial )
    {
        name = @"OnInterstitialHiddenEvent";
    }
    else // REWARDED
    {
        name = @"OnRewardedAdHiddenEvent";
    }
    
    [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": name,
                                                   @"adUnitId": ad.adUnitIdentifier}];
}

- (void)didCollapseAd:(MAAd *)ad
{
    if ( ad.format != MAAdFormat.banner && ad.format != MAAdFormat.mrec && ad.format != MAAdFormat.leader )
    {
        [self logInvalidAdFormat: ad.format];
        return;
    }
    
    [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": @"OnBannerAdCollapsedEvent",
                                                   @"adUnitId": ad.adUnitIdentifier}];
}

- (void)didExpandAd:(MAAd *)ad
{
    if ( ad.format != MAAdFormat.banner && ad.format != MAAdFormat.leader )
    {
        [self logInvalidAdFormat: ad.format];
        return;
    }
    
    [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": @"OnBannerAdExpandedEvent",
                                                   @"adUnitId": ad.adUnitIdentifier}];
}

- (void)didCompleteRewardedVideoForAd:(MAAd *)ad
{
    // This event is not forwarded
}

- (void)didStartRewardedVideoForAd:(MAAd *)ad
{
    // This event is not forwarded
}

- (void)didRewardUserForAd:(MAAd *)ad withReward:(MAReward *)reward
{
    if ( ad.format != MAAdFormat.rewarded )
    {
        [self logInvalidAdFormat: ad.format];
        return;
    }
    
    NSString *rewardLabel = reward ? reward.label : @"";
    NSInteger rewardAmountInt = reward ? reward.amount : 0;
    NSString *rewardAmount = [@(rewardAmountInt) stringValue];
    
    [MAUnityAdManager forwardUnityEventWithArgs: @{@"name": @"OnRewardedAdReceivedRewardEvent",
                                                   @"adUnitId": ad.adUnitIdentifier,
                                                   @"rewardLabel": rewardLabel,
                                                   @"rewardAmount": rewardAmount}];
}

#pragma mark - Internal Methods

- (void)logInvalidAdFormat:(MAAdFormat *) adFormat
{
    [self log: @"invalid ad format: %@, from %@", adFormat, [NSThread callStackSymbols]];
}

- (void)log:(NSString *)format, ...
{
    va_list valist;
    va_start(valist, format);
    NSString *message = [[NSString alloc] initWithFormat: format arguments: valist];
    va_end(valist);
    
    NSLog(@"[%@] %@", TAG, message);
}

- (MAInterstitialAd *)retrieveInterstitialForAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    MAInterstitialAd *result = self.interstitials[adUnitIdentifier];
    if ( !result )
    {
        result = [[MAInterstitialAd alloc] initWithAdUnitIdentifier: adUnitIdentifier sdk: self.sdk];
        result.delegate = self;
        
        self.interstitials[adUnitIdentifier] = result;
    }
    
    return result;
}

- (MARewardedAd *)retrieveRewardedAdForAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    MARewardedAd *result = self.rewardedAds[adUnitIdentifier];
    if ( !result )
    {
        result = [MARewardedAd sharedWithAdUnitIdentifier: adUnitIdentifier sdk: self.sdk];
        result.delegate = self;
        
        self.rewardedAds[adUnitIdentifier] = result;
    }
    
    return result;
}

- (MAAdView *)retrieveAdViewForAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    return [self retrieveAdViewForAdUnitIdentifier: adUnitIdentifier atPosition: nil];
}

- (MAAdView *)retrieveAdViewForAdUnitIdentifier:(NSString *)adUnitIdentifier atPosition:(NSString *)adViewPosition
{
    MAAdView *result = self.adViews[adUnitIdentifier];
    if ( !result && adViewPosition )
    {
        result = [[MAAdView alloc] initWithAdUnitIdentifier: adUnitIdentifier sdk: self.sdk];
        result.translatesAutoresizingMaskIntoConstraints = NO;
        result.delegate = self;
        
        self.adViews[adUnitIdentifier] = result;
        self.adViewPositions[adUnitIdentifier] = adViewPosition;
        
        UIViewController *rootViewController = [MAUnityAdManager unityViewController];
        [rootViewController.view addSubview: result];
        
        [self positionBannerForAdView: result adViewPosition: adViewPosition];
    }
    
    return result;
}

- (void)positionBannerForAdUnitIdentifier:(NSString *)adUnitIdentifier
{
    MAAdView *adView = [self retrieveAdViewForAdUnitIdentifier: adUnitIdentifier];
    NSString *adViewPosition = self.adViewPositions[adUnitIdentifier];
    
    [self positionBannerForAdView: adView adViewPosition: adViewPosition];
}

- (void)positionBannerForAdView:(MAAdView *)adView adViewPosition:(NSString *)adViewPosition
{
    UIView *superview = adView.superview;
    if ( !superview ) return;
    
    // Ensure superview contains the safe area background
    if ( ![superview.subviews containsObject: self.safeAreaBackground] )
    {
        [self.safeAreaBackground removeFromSuperview];
        [superview insertSubview: self.safeAreaBackground belowSubview: adView];
    }
    
    CGSize bannerSize = [MAUnityAdManager bannerSize];
    
    // All positions have constant height
    NSMutableArray<NSLayoutConstraint*> *constraints = [NSMutableArray arrayWithObject: [adView.heightAnchor constraintEqualToConstant: bannerSize.height]];
    
    UILayoutGuide *layoutGuide;
    if ( @available(iOS 11.0, *) )
    {
        layoutGuide = superview.safeAreaLayoutGuide;
    }
    else
    {
        layoutGuide = superview.layoutMarginsGuide;
    }
    
    // If top of bottom center, stretch width of screen
    if ( [adViewPosition isEqual: @"TopCenter"] || [adViewPosition isEqual: @"BottomCenter"] )
    {
        [constraints addObjectsFromArray: @[[self.safeAreaBackground.leftAnchor constraintEqualToAnchor: superview.leftAnchor],
                                            [self.safeAreaBackground.rightAnchor constraintEqualToAnchor: superview.rightAnchor]]];
        
        if ( [adViewPosition isEqual: @"TopCenter"] )
        {
            [constraints addObjectsFromArray: @[[adView.topAnchor constraintEqualToAnchor: layoutGuide.topAnchor],
                                                [adView.leftAnchor constraintEqualToAnchor: superview.leftAnchor],
                                                [adView.rightAnchor constraintEqualToAnchor: superview.rightAnchor]]];
            [constraints addObjectsFromArray: @[[self.safeAreaBackground.topAnchor constraintEqualToAnchor: superview.topAnchor],
                                                [self.safeAreaBackground.bottomAnchor constraintEqualToAnchor: adView.topAnchor]]];
        }
        else // BottomCenter
        {
            [constraints addObjectsFromArray: @[[adView.bottomAnchor constraintEqualToAnchor: layoutGuide.bottomAnchor],
                                                [adView.leftAnchor constraintEqualToAnchor: superview.leftAnchor],
                                                [adView.rightAnchor constraintEqualToAnchor: superview.rightAnchor]]];
            [constraints addObjectsFromArray: @[[self.safeAreaBackground.topAnchor constraintEqualToAnchor: adView.bottomAnchor],
                                                [self.safeAreaBackground.bottomAnchor constraintEqualToAnchor: superview.bottomAnchor]]];
        }
    }
    // Otherwise, publisher will likely construct his own views around the adview
    else
    {
        self.safeAreaBackground.hidden = YES;
        
        // Assign constant width of 320 or 728
        [constraints addObject: [adView.widthAnchor constraintEqualToConstant: bannerSize.width]];
        
        if ( [adViewPosition isEqual: @"TopLeft"] )
        {
            [constraints addObjectsFromArray: @[[adView.topAnchor constraintEqualToAnchor: layoutGuide.topAnchor],
                                                [adView.leftAnchor constraintEqualToAnchor: superview.leftAnchor]]];
        }
        else if ( [adViewPosition isEqual: @"TopRight"] )
        {
            [constraints addObjectsFromArray: @[[adView.topAnchor constraintEqualToAnchor: layoutGuide.topAnchor],
                                                [adView.rightAnchor constraintEqualToAnchor: superview.rightAnchor]]];
        }
        else if ( [adViewPosition isEqual: @"Centered"] )
        {
            [constraints addObjectsFromArray: @[[adView.centerXAnchor constraintEqualToAnchor: layoutGuide.centerXAnchor],
                                                [adView.centerYAnchor constraintEqualToAnchor: layoutGuide.centerYAnchor]]];
        }
        else if ( [adViewPosition isEqual: @"BottomLeft"] )
        {
            [constraints addObjectsFromArray: @[[adView.bottomAnchor constraintEqualToAnchor: layoutGuide.bottomAnchor],
                                                [adView.leftAnchor constraintEqualToAnchor: superview.leftAnchor]]];
        }
        else if ( [adViewPosition isEqual: @"BottomRight"] )
        {
            [constraints addObjectsFromArray: @[[adView.bottomAnchor constraintEqualToAnchor: layoutGuide.bottomAnchor],
                                                [adView.rightAnchor constraintEqualToAnchor: superview.rightAnchor]]];
        }
    }
    
    [NSLayoutConstraint activateConstraints: constraints];
}

+ (UIViewController *)unityViewController
{
    return [[[UIApplication sharedApplication] keyWindow] rootViewController];
}

+ (void)forwardUnityEventWithArgs:(NSDictionary<NSString *, NSString *> *)args
{
    NSString *serializedParameters = [self propsStrFromDictionary: args];
    UnitySendMessage("MaxSdkCallbacks", "ForwardEvent", serializedParameters.UTF8String);
}

+ (NSString *)propsStrFromDictionary:(NSDictionary<NSString *, NSString *> *)dict
{
    NSMutableString *result = [[NSMutableString alloc] initWithCapacity: 64];
    [dict enumerateKeysAndObjectsUsingBlock:^(NSString *key, NSString *obj, BOOL *stop)
     {
         [result appendString: key];
         [result appendString: @"="];
         [result appendString: obj];
         [result appendString: @"\n"];
     }];
    
    return result;
}

- (NSDictionary<NSString *, NSString *> *)deserializeEventParameters:(NSString *)serialized
{
    if ( serialized.length > 0 )
    {
        NSArray<NSString *> *keyValuePairs = [serialized componentsSeparatedByString: ALSerializeKeyValuePairSeparator]; // ["key-1<FS>value-1", "key-2<FS>value-2", "key-3<FS>value-3"]
        NSMutableDictionary<NSString *, NSString *> *deserialized = [NSMutableDictionary dictionaryWithCapacity: keyValuePairs.count];
        
        for ( NSString *keyValuePair in keyValuePairs )
        {
            NSArray<NSString *> *splitPair = [keyValuePair componentsSeparatedByString: ALSerializeKeyValueSeparator];
            if ( splitPair.count == 2 )
            {
                NSString *key = splitPair[0];
                NSString *value = splitPair[1];
                
                // Store in deserialized dictionary
                deserialized[key] = value;
            }
        }
        
        return deserialized;
    }
    else
    {
        return @{};
    }
}

+ (CGSize)bannerSize
{
    if ( [UIDevice currentDevice].userInterfaceIdiom == UIUserInterfaceIdiomPad )
    {
        return CGSizeMake(728.0f, 90.0f);
    }
    else
    {
        return CGSizeMake(320.0f, 50.0f);
    }
}

@end
