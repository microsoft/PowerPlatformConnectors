DeepL's mission is to overcome language barriers and bring cultures closer together. In order to achieve that mission, we use our expertise in artificial intelligence and neural networks to create technologies that make communication faster, better, and easier.

The DeepL API Connector allows you to use the DeepL API in your flows. Translate text or documents on the fly or manage your glossaries. Any functionality we offer with DeepL API can be used in Power Automate as well. 


##  Prerequisites  &  Credentials

To use the connector, a DeepL API Free or Pro subscription is required. [You  can  sign  up  here.](https://www.deepl.com/pro-api?utm_source=power-automate&utm_medium=connectors&utm_campaign=power-automate-intro) After signing up, you can find your authentication key in the "Account" tab in your DeepL API account.

While creating your flow, you'll be asked to specify whether you have a Free or Pro plan. This has to be selected accordingly. 


##  Get  started  with  your  connector  

Start to add a new flow as usually and choose one of the available DeepL actions. Doing that for the first time will ask you to add some connection details as seen below:

Choose the price plan accordingly and add your API Key. The name can be anything you want. Confirm the step with "Create" and after that you can use the connection for all other actions available. 


##  Known  issues  and  limitations  

*  We  can  not  auto-detect  your  price  plan.  You  have  to  choose  between  Free  and  Pro  yourself  when  setting  up  a  flow  for  the  first  time.  

*  Some  endpoints  do  not  support  JSON  but  only  work  with  URLFormEncoded  content,  and  while  the  connector  has  an  internal  workaround,  this  might  not  always  work  for  everyone.  
​

##  Common  errors  and  remedies

 *  HTTP  429  -  A  429  error  can  be  caused  by  multiple  issues:  either  you  tried  to  call  the  API  too  often,  or  there  might  be  a  temporary  issue  on  our  end.  The  general  recommendation  is  to  use  an  exponential  back-off  approach  and  retry  the  call.  Exponential  back-off  is  not  implemented  by  the  connector  itself.  

 *  HTTP  400  -  Something  was  wrong  with  your  call,  payload,  or  parameters  used.

 *  HTTP  456  -  Quota  exceeded.  If  you  get  a  456  error,  your  account  limit  for  characters  has  been  reached  and  you  need  to  either  wait  until  the  next  usage  period  or  upgrade  your  account.  If  you're  a  Free  API  user,  you  can  upgrade  [in  the  "Plan"  tab  of  your  account.](https://www.deepl.com/account/plan)



##  FAQ  

*  What  is  the  difference  between  DeepL  API  Free  and  Pro  plans?

DeepL API Free is a variant of our DeepL API Pro plan that lets you translate up to 500,000 characters per month for free. 

For more advanced needs, the DeepL API Pro plan offers the following additional benefits:

1.  Maximum  data  security  (your  texts  are  deleted  immediately  after  the  translation)

2.  No  volume  restriction  on  the  number  of  characters  you  can  translate  per  month  (a  billing  model  based  on  a  fixed  price  of  €4.99  per  month  +  usage-based  costs  of  €0.00002  per  translated  character)

3.  Prioritized  execution  of  translation  requests

​

[Learn  more  in  our  help  center.](https://support.deepl.com/hc/en-us/articles/360021183620-DeepL-API-Free-vs-DeepL-API-Pro)



*  What  if  I  reached  my  character  limit  on  the  Free  plan?

If you reached your monthly character limit on Deepl API Free, you'll have to wait until the next usage period (i.e. month) to be able to use the API again. Alternatively, you can consider upgrading to a Pro API subscription [in  the  "Plan"  tab  of  your  account.](https://www.deepl.com/account/plan)



*  As  a  Pro  subscriber,  is  there  a  way  to  limit  my  usage-based  costs?

Yes, you can activate the cost control feature from [the  "Plan"  tab  of  your  Pro  account.](https://www.deepl.com/account/plan). Learn more about cost control [in  our  help  center.](https://support.deepl.com/hc/en-us/articles/360020685580-Cost-control) 
​

*  Which  functionality  is  available  using  the  connector?

Anything you can currently do with our API can be done using the connector as well. You can look [here](https://www.deepl.com/docs-api) to see all supported functionality and additional documentation about parameters and details.

