% MATLAB implementation test
function mlegaussfit()

    % Parameters format: X, Y, Sigma, I_0, I_bg

    W = 60; H =60;
    Pcenter = [ W/2 H/2 4 10000 10 ];  %
    [img, imgcv] = makesample ([H W], Pcenter);

    imshow([ normalize(img) normalize(imgcv) ]);
    
    % Localize
    N = 0; 
    iterations = 10;
    for k = 1 : N
        P = Pcenter+(rand(1,5)-.5).*[5 5 1 300 5 ];
        Pinitial = P+(rand(1,5)-.5).*[10 10 2 300 10 ];
        
        [~,smp] = makesample([H W], P);
        Pestimate = fitgauss(smp, Pinitial, iterations);

        d=P-Pestimate;
        dist(k) = sqrt (dot( d(1:2), d(1:2) ));
        fprintf('dist [%d]: %f\n', k, dist(k));
    end
    
end

function Pestimate = fitgauss(smpimg, P, iterations)

    dim = size(smpimg);
    [X,Y] = meshgrid(0:dim(2)-1,0:dim(1)-1);
    
    for k=1:iterations
        Sx = P(1); Sy = P(2); Sigma = P(3); I0 = P(4); Ibg = P(5);
        
        Xexp0 = (X-Sx + .5) / (sqrt(2)*Sigma);
        Yexp0 = (Y-Sy + .5) / (sqrt(2)*Sigma);
        
        Xexp1 = (X-Sx - .5) / (sqrt(2)*Sigma);
        Yexp1 = (Y-Sy - .5) / (sqrt(2)*Sigma);

        DeltaX = 0.5 * erf( Xexp0 ) - 0.5 * erf(Xexp1);
        DeltaY = 0.5 * erf( Yexp0 ) - 0.5 * erf(Yexp1);
        mu = Ibg + I0 * DeltaX .* DeltaY;
        
        dmu_dx = I0/(sqrt(2*pi)*Sigma) * ( exp(-Xexp1.^2) - exp(-Xexp0.^2 ) ) .* DeltaY;
        dmu_dy = I0/(sqrt(2*pi)*Sigma) * ( exp(-Yexp1.^2) - exp(-Yexp0.^2 ) ) .* DeltaX;
        dmu_dI0 = DeltaX.*DeltaY;
        dmu_dIbg = 1;
        
        d2mu_dx = I0/(sqrt(2*pi)*Sigma.^3) * ( (X - Sx - .5) .* exp (Xexp1.^2) - (X - Sx + .5) .* exp (Xexp0.^2) ) .* DeltaY;
        d2mu_dy = I0/(sqrt(2*pi)*Sigma.^3) * ( (Y - Sy - .5) .* exp (Yexp1.^2) - (Y - Sy + .5) .* exp (Yexp0.^2) ) .* DeltaX;
        %d2mu_dI0 = 0
        %d2mu_dIbg = 0
 
 %       imshow([ normalize(mu) normalize(dmu_dx) normalize(dmu_dy)] );
        
        dL_dx = dmu_dx .* ( smpimg ./ mu - 1 );
        dL_dy = dmu_dy .* ( smpimg ./ mu - 1 );
        dL_dI0 = dmu_dI0 .* ( smpimg ./ mu - 1 );
        dL_dIbg = dmu_dIbg .* ( smpimg ./ mu - 1 );

        imshow([ normalize(mu) normalize(dmu_dx) normalize(dmu_dy); ...
            normalize(smpimg) normalize(dL_dx) normalize(dL_dy)] );
        
        dL = [ sum(dL_dx(:)) sum(dL_dy(:)) 0 sum(dL_dI0(:)) sum(dL_dIbg(:)) ];
      %  dL = [ sum(dL_dx(:)) sum(dL_dy(:)) 0 0 0 ];
        
        dL2_dx = d2mu_dx .* ( smpimg ./ mu - 1 ) - dmu_dx.^2 .* smpimg ./ (mu.^2);
        dL2_dy = d2mu_dy .* ( smpimg ./ mu - 1 ) - dmu_dy.^2 .* smpimg ./ (mu.^2);
        dL2_dI0 = - dmu_dI0.^2 .* smpimg ./ (mu.^2);
        dL2_dIbg = - smpimg ./ (mu.^2);
        
        dL2 = [ sum(dL2_dx(:)) sum(dL2_dy(:)) 1 sum(dL2_dI0(:)) sum(dL2_dIbg(:)) ];
        
        DeltaP = 0.005*dL;% ./ dL2;
      %  fprintf('dx: %f, dy: %f, dI0: %f\n', DeltaP(1), DeltaP(2), DeltaP(4));
        P = P + DeltaP;
      %  pause
        
    end
    
    Pestimate = P;
end


function [img, imgcv] = makesample(Size, P)
    [Y,X] = ndgrid (0:Size(1)-1, 0:Size(2)-1);
    
    % Read parameters
    Sx = P(1); Sy = P(2); Sigma = P(3); I0 = P(4); Ibg = P(5);
    
    % Center value:
    imgcv = Ibg + I0 * exp ( - ((X-Sx).^2+(Y-Sy).^2) / (2*Sigma^2) ) / (2*pi*Sigma^2);
    imgcv = poissrnd(imgcv);
    
    % Expected values
    edenom = 1/sqrt(2*Sigma^2);
    DeltaX = 0.5 * erf( (X-Sx + .5) * edenom ) - 0.5 * erf((X-Sx - .5) * edenom);
    DeltaY = 0.5 * erf( (Y-Sy + .5) * edenom ) - 0.5 * erf((Y-Sy - .5) * edenom );
    img = Ibg + I0 * DeltaX .* DeltaY;
    
    img = poissrnd(img);
end



function d = normalize(d)
    minv = min(d(:));
    maxv = max(d(:));
    
    d = (d-minv) ./ (maxv-minv);
end
